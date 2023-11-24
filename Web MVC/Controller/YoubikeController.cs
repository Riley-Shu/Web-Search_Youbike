using Microsoft.AspNetCore.Mvc;
using Sample04_Web.Models;

namespace Sample04_Web.Controllers
{
    public class YoubikeController : Controller
    {
        //Data Field
        private ServiceEntity _serviceEntity;
        private Sarea _sarea;
        //Ctor injection
        public YoubikeController(ServiceEntity serviceEntity, Sarea sarea)
        {
            _serviceEntity = serviceEntity;
            _sarea = sarea;
        }

        //頁面
        public IActionResult qryByArea()
        {
            //Part1: 建立ViewData，添加url
            String url = this._serviceEntity.youbikeService;
            //Console.WriteLine($"url:{url}");
            ViewData["youbikeServiceUrl"] = url;

            //Part2: 建立ViewData，添加sareaList
            String[] TaipeiList = this._sarea.Taipei;
            //foreach (String i in TaipeiList)
            //{
            //    Console.WriteLine(i);
            //}
            ViewData["TaipeiList"] = TaipeiList;
            //List<String> TaipeiList = this._sarea.Taipei;
            //ViewBag.sareaList = TaipeiList;
            //說明: 考慮到行政區變動機會少，不使用 List<String>,ViewBag，而是使用String[],ViewData
            //注意: 更換結構類型等時，需檢查相關提取，否則會找不到資料

            //回傳View
            return View();
        }
    }
}
