﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using FluentValidation;
using Reminders.Application.Contracts;
using Reminders.Application.Validators.Reminders;
using Reminders.Application.Validators.Reminders.Exceptions;
using Reminders.Application.Validators.Reminders.Exceptions.Enumerables;
using Reminders.Application.ViewModels;
using Reminders.Domain.Contracts;
using Reminders.Domain.Contracts.Repositories;
using Reminders.Domain.Models;
using System;
using System.Linq;

namespace Reminders.Application.Services
{
    // TODO: Enhance way to send back validation result.
    public class RemindersService
        : IRemindersService
    {
        private readonly ReminderValidator validator;
        private readonly IMapper mapper;
        private readonly IRemindersRepository remindersRepository;
        private readonly IUnitOfWork unitOfWork;

        public RemindersService(
            IMapper mapper,
            IRemindersRepository remindersRepository,
            IUnitOfWork unitOfWork)
        {
            validator = new ReminderValidator();
            this.mapper = mapper;
            this.remindersRepository = remindersRepository;
            this.unitOfWork = unitOfWork;
        }

        public void Insert(ReminderViewModel reminderViewModel)
        {
            reminderViewModel.IsDone = false;

            var reminder = mapper.Map<Reminder>(reminderViewModel);

            validator.ValidateAndThrow(reminder, ruleSet: "*");

            remindersRepository.Add(reminder);

            unitOfWork.Commit();
        }

        public void Edit(Guid id, ReminderViewModel reminderViewModel)
        {
            if (id != reminderViewModel.Id)
                throw new RemindersApplicationException(StatusCode.IdsDoNotMatch, RemindersResources.IdsDoNotMatch);

            if (!remindersRepository.Exists(id))
                throw new RemindersApplicationException(StatusCode.NotFound, RemindersResources.NotFound);

            var reminder = mapper.Map<Reminder>(reminderViewModel);

            validator.ValidateAndThrow(reminder);

            remindersRepository.Update(reminder);

            unitOfWork.Commit();
        }

        public void Delete(Guid id)
        {
            if (!remindersRepository.Exists(id))
                throw new RemindersApplicationException(StatusCode.NotFound, RemindersResources.NotFound);

            var reminderData = remindersRepository.Get(id);

            reminderData.Delete();

            remindersRepository.Update(reminderData);

            unitOfWork.Commit();
        }

        public IQueryable<ReminderViewModel> Get() =>
            remindersRepository.Get()
                .Where(r => !r.IsDeleted)
                .ProjectTo<ReminderViewModel>(mapper.ConfigurationProvider);

        public ReminderViewModel Get(Guid id) =>
            mapper.Map<ReminderViewModel>(
                remindersRepository.Get()
                .FirstOrDefault(r => r.Id == id && !r.IsDeleted));

        public IQueryable<ReminderViewModel> GetInactive() =>
            remindersRepository.Get()
                .Where(r => r.IsDeleted)
                .ProjectTo<ReminderViewModel>(mapper.ConfigurationProvider);

        public ReminderViewModel GetInactive(Guid id) =>
            mapper.Map<ReminderViewModel>(
                remindersRepository.Get()
                .FirstOrDefault(r => r.Id == id && r.IsDeleted));
    }
}