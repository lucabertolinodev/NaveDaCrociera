﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NaveDaCrociera.DB;
using NaveDaCrociera.DB.Entities;
using NaveDaCrociera.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace NaveDaCrociera.Controllers
{
    public class HomeController : Controller
    {
        //private readonly ILogger<HomeController> _logger;

        //public HomeController(ILogger<HomeController> logger)
        //{
        //    _logger = logger;
        //}
        private readonly Repository repository;
        private SignInManager<User> signInManager;
        private UserManager<User> userManager;
        private UserDBContext dbContext;
        public HomeController(SignInManager<User> signInManager,
            UserManager<User> userManager,
            UserDBContext dbContext, Repository repository)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.dbContext = dbContext;
            this.repository = repository;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }
        public IActionResult Eventi()
        {
            List<Spettacoli> spettacoli = this.repository.GetSpettacoli();
            List<SpettacoliModel> model = new List<SpettacoliModel>();
            foreach (Spettacoli p in spettacoli)
                model.Add(new SpettacoliModel()
                {
                    Id = p.Id.ToString(),
                    NomeEvento = p.NomeEvento,
                    Locale = p.Locale,
                    Posti = p.Posti.ToString(),
                    Annullato = p.Annullato
                });
            return View(model);                       
        }

        public IActionResult PrenotazioniSpettacoli()
        {
            List<PrenotazioniSpettacoli> prenotazioniSpettacoli = this.repository.GetPrenotazioniSpettacoli();
            List<PrenotazioniSpettacoliModel> model = new List<PrenotazioniSpettacoliModel>();
            foreach (PrenotazioniSpettacoli p in prenotazioniSpettacoli)
                model.Add(new PrenotazioniSpettacoliModel()
                {
                    Id = p.Id,
                    NomeEvento = p.NomeEvento,
                    Locale = p.Locale,
                    PostiPre = p.PostiPre                    
                });
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            try
            {
                User user = await userManager.FindByNameAsync(loginModel.UserName);
                if (user != null)
                {
                    var result = await signInManager.PasswordSignInAsync(loginModel.UserName, loginModel.Password, false, lockoutOnFailure: true);
                    if (result.Succeeded)
                    {

                    }
                }
            }
            catch (Exception ex)
            {

            }
            return Redirect("Index");
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            try
            {
                if (signInManager.IsSignedIn(User))
                {
                    await signInManager.SignOutAsync();
                }
            }
            catch (Exception ex)
            {
            }
            return Redirect("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
