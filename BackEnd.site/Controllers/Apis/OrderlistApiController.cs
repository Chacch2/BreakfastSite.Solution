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
    }


        //[HttpGet]
        //[Route("api/orders/{orderId}")]
        //public IHttpActionResult GetOrderById(int orderId)
        //{
        //    var db = new AppDbContext();

        //    // 查询指定订单 ID 的订单详情
        //    var order = db.Orders
        //        .Where(o => o.Id == orderId)
        //        .Select(o => new OrderVm
        //        {
        //            OrderID = o.Id,
        //            OrderTime = o.OrderTime,
        //            PickupTime = o.TakeTime,
        //            TotalAmount = o.Total,
        //            OrderStatus = o.OrderStatus,
        //            PointsUsed = o.PointsUsed,
        //            PointsEarned = o.PointsEarned,

        //            // 查询 OrderDetails 的数据
        //            Items = db.OrderDetails
        //                .Where(od => od.OrderID == o.Id)
        //                .Select(od => new OrderItemVm
        //                {
        //                    Name = od.Product.Name,
        //                    Description = od.Product.Name, // 假设使用产品名称作为描述
        //                    Price = od.ProductPrice,
        //                    Quantity = od.ProductQuantity,
        //                    Image = od.Product.Image,

        //                    // 加选项的查询
        //                    //Extras = db.OrderAddOnDetails
        //                    //    .Where(aod => aod.OrderDetailID == od.Id)
        //                    //    .Select(aod => new AddOnVm
        //                    //    {
        //                    //        Name = aod.ProductAddOnDetails.AddOnOption.Name,
        //                    //        Price = aod.AddOnOptionPrice
        //                    //    }).ToList()
        //                }).ToList()
        //        }).FirstOrDefault();

        //    if (order == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(order);
        //}

    }

