using BackEnd.site.Models.EFModels;
using BackEnd.site.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BackEnd.site.Controllers.Apis
{

    public class OrderlistApiController : ApiController
    {
        [HttpGet]
        [Route("api/orders/all")]
        public IHttpActionResult Get()
        {
            var db = new AppDbContext();

            // 查询指定订单 ID 的订单详情
            var order = db.Orders
                .Select(o => new OrderVm
                {
                    OrderID = o.Id,
                    OrderTime = o.OrderTime,
                    PickupTime = o.TakeTime,
                    TotalAmount = o.FinalTotal,
                    OrderStatus = o.OrderStatus,

                    // PointsEarned 从 PointDetails 中提取
                    PointsEarned = db.PointDetails
                        .Where(pd => pd.OrderId == o.Id)
                        .Select(pd => pd.Earned)
                        .FirstOrDefault(),

                    PointsUsed = o.PointsUsed,

                    // 查询 OrderDetails 的数据
                    Items = db.OrderDetails
                        .Where(od => od.OrderID == o.Id)
                        .Select(od => new OrderItemVm
                        {
                            Name = od.Product.Name,

                            // Description 仅提取相关加选项的资料，不包括价格
                            AddOns = db.OrderAddOnDetails
                                .Where(aod => aod.OrderDetailID == od.Id)
                                .Select(aod => new AddOnVm
                                {
                                    AddOnOptionName = db.ProductAddOnDetails
                                        .Where(pad => pad.Id == aod.ProductAddOnDetailsID)
                                        .Select(pad => pad.AddOnOptionName)
                                        .FirstOrDefault()
                                }).ToList(),

                            // 从 Products 表提取 Image
                            Image = db.Products
                                .Where(p => p.Name == od.Product.Name)
                                .Select(p => p.Image)
                                .FirstOrDefault(),

                            Price = od.ProductPrice,
                            Quantity = od.ProductQuantity
                        }).ToList()
                })
                .ToList();  // 先把資料提取出來

            // 在记忆体中进行操作
            foreach (var ord in order)
            {
                foreach (var item in ord.Items)
                {
                    // 使用 string.Join 将 AddOnOptionName 相关信息组合起来
                    item.Description = string.Join(", ", item.AddOns.Select(a => a.AddOnOptionName));
                }
            }

            if (order == null || !order.Any())  // 检查列表是否为空
            {
                return NotFound();
            }

            return Ok(order);
        }






        [HttpPost]
        [Route("api/orders/cancel/{orderId}")]
        public IHttpActionResult CancelOrder(int orderId)
        {
            using (var db = new AppDbContext())
            {
                // 找到對應的訂單
                var order = db.Orders.FirstOrDefault(o => o.Id == orderId);

                if (order == null)
                {
                    // 如果找不到訂單，返回 404 Not Found
                    return NotFound();
                }

                // 檢查訂單狀態是否允許取消
                if (order.OrderStatus != 1)
                {
                    // 如果不是 "未取餐" 狀態，則不能取消訂單，返回 400 Bad Request
                    return BadRequest("訂單已經被處理，無法取消。");
                }

                // 更新訂單狀態為 "已取消" (3)
                order.OrderStatus = 3;

                // 儲存變更到資料庫
                db.SaveChanges();

                // 返回成功訊息
                return Ok(new { message = "訂單已取消", orderId = orderId });
            }
        }

    }






}

