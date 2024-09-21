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
                    TotalAmount = o.Total,
                    OrderStatus = o.OrderStatus,
                    PointsUsed = o.PointsUsed,
                    PointsEarned = o.PointsEarned,

                    // 查询 OrderDetails 的数据
                    Items = db.OrderDetails
                        .Where(od => od.OrderID == o.Id)
                        .Select(od => new OrderItemVm
                        {
                            Name = od.Product.Name,
                            Description = od.Product.Name, // 假设使用产品名称作为描述
                            Price = od.ProductPrice,
                            Quantity = od.ProductQuantity,
                            Image = od.Product.Image,

                            // 加选项的查询
                            //Extras = db.OrderAddOnDetails
                            //    .Where(aod => aod.OrderDetailID == od.Id)
                            //    .Select(aod => new AddOnVm
                            //    {
                            //        Name = aod.ProductAddOnDetails.AddOnOption.Name,
                            //        Price = aod.AddOnOptionPrice
                            //    }).ToList()
                        }).ToList()
                });

            if (order == null)
            {
                return NotFound();
            }

            return Ok(order);
        }


        [HttpPut]
        [Route("api/orders/cancel")]
        public IHttpActionResult CancelOrder([FromBody] OrderUpdateVm orderUpdate)
        {
            var db = new AppDbContext();
            // 根據訂單 ID 查詢訂單
            var order = db.Orders.SingleOrDefault(o => o.Id == orderUpdate.OrderID);

            if (order == null)
            {
                return NotFound(); // 如果訂單不存在，返回 404
            }

            // 更新訂單狀態為已取消
            order.OrderStatus = orderUpdate.OrderStatus;

            try
            {
                db.SaveChanges(); // 保存修改到資料庫
                return Ok(order); // 返回成功的結果
            }
            catch (Exception ex)
            {
                return BadRequest("更新失敗：" + ex.Message); // 返回失敗的訊息
            }
        }
    }






}

