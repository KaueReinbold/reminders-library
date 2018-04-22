﻿using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Reminders.Business.Contracts;
using Reminders.Domain;
using Reminders.Domain.Enums;
using Reminders.Domain.Models;

namespace Reminders.Mvc.Controllers
{
    public class RemindersController : Controller
    {
        private readonly ILogger<RemindersController> _logger;
        private readonly IBusinessModelGeneric<ReminderModel> _businessReminderModel;

        public RemindersController(ILogger<RemindersController> logger, IBusinessModelGeneric<ReminderModel> businessReminderModel)
        {
            _logger = logger;
            _businessReminderModel = businessReminderModel;
        }

        // GET: Reminders
        public ActionResult Index()
        {
            var reminders = _businessReminderModel.GetAll();

            return View(reminders);
        }

        // GET: Reminders/Details/5
        public ActionResult Details(int id)
        {
            var reminder = _businessReminderModel.Find(id);

            return View(reminder);
        }

        // GET: Reminders/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Reminders/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ReminderModel reminderModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _businessReminderModel.Insert(reminderModel);

                    TempData["Message"] = JsonConvert.SerializeObject(new MessageModel { Type = EnumMessages.Success, Message = "Reminder has created!" });
                    
                    return RedirectToAction(nameof(Index));
                }
                else
                    return View(reminderModel);

            }
            catch (Exception ex)
            {
                TempData["Message"] = JsonConvert.SerializeObject(new MessageModel { Type = EnumMessages.Error, Message = "Some error has happened!" });

                _logger.LogError(ex, string.Empty);

                return View();
            }
        }

        // GET: Reminders/Edit/5
        public ActionResult Edit(int id)
        {
            var reminder = _businessReminderModel.Find(id);

            return View(reminder);
        }

        // POST: Reminders/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ReminderModel reminderModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _businessReminderModel.Update(reminderModel);

                    TempData["Message"] = JsonConvert.SerializeObject(new MessageModel { Type = EnumMessages.Success, Message = "Reminder has updated!" });
                    
                    return RedirectToAction(nameof(Index));
                }
                else
                    return View(reminderModel);
            }
            catch (Exception ex)
            {
                TempData["Message"] = JsonConvert.SerializeObject(new MessageModel { Type = EnumMessages.Error, Message = "Some error has happened!" });

                _logger.LogError(ex, string.Empty);

                return View();
            }
        }

        // GET: Reminders/Delete/5
        public ActionResult Delete(int id)
        {
            var reminder = _businessReminderModel.Find(id);

            return View(reminder);
        }

        // POST: Reminders/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(ReminderModel reminderModel)
        {
            try
            {
                _businessReminderModel.Delete(reminderModel.Id);
                
                TempData["Message"] = JsonConvert.SerializeObject(new MessageModel { Type = EnumMessages.Success, Message = "Reminder has deleted!" });

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Message"] = JsonConvert.SerializeObject(new MessageModel { Type = EnumMessages.Error, Message = "Some error has happened!" });

                _logger.LogError(ex, string.Empty);

                return View();
            }
        }
    }
}