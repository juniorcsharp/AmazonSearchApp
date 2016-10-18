using AmazonSearch.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UptimeTestUlesanne.Controllers;

namespace AmazonSearch.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Search(string keyword)
        {
            ViewBag.Keyword = keyword;
            List<SearchedItem> searchedItems = new List<SearchedItem>();
            for(int i = 1; i < 3; i ++)
            {
                searchedItems.AddRange(ItemSearch.ItemSearchResponse(ItemSearch.ItemSearchRequest(keyword, i)));
            }

            int id = 1;
            foreach (SearchedItem item in searchedItems)
            {
                item.Id = id;
                id++;
            }

            return View(searchedItems);
        }
    }
}