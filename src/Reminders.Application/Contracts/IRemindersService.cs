﻿using Reminders.Application.ViewModels;
using System;
using System.Linq;

namespace Reminders.Application.Contracts
{
    public interface IRemindersService
    {
        void Insert(ReminderViewModel reminderViewModel);
        void Edit(Guid id, ReminderViewModel reminderViewModel);
        void Delete(Guid id);

        IQueryable<ReminderViewModel> Get();
        ReminderViewModel Get(Guid id);
    }
}