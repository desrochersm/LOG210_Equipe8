﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebResto.Models;

namespace WebResto.Controllers
{
    public class CompteController : Controller
    {
        // GET: Account
        [HttpGet]
        public ActionResult Authentification()
        {
            return View();
        }

        public ActionResult Authentification(Client client)
        {
            return null;
        }
    }
}