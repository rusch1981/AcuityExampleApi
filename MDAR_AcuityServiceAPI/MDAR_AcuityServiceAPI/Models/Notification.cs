using System;
using System.Net.Http.Formatting;

namespace MDAR_AcuityServiceAPI.Models
{
    public class Notification
    {
        private string _asString;


        public string Action { get; private set; }
        public string Id { get; private set; }
        public string CalendarId { get; private set; }
        public string AppointmentTypeId { get; private set; }
        public bool IsValid { get; private set; }

        #region PublicConstructor

        public Notification(FormDataCollection form)
        {
            ValidateForm(form);
            Initalize(form);
        }

        #endregion

        #region Private Methods

        private void ValidateForm(FormDataCollection form)
        {
            if (string.IsNullOrWhiteSpace(form?.Get("action")) ||
                string.IsNullOrWhiteSpace(form.Get("id")) ||
                string.IsNullOrWhiteSpace(form.Get("calendarID")) ||
                string.IsNullOrWhiteSpace(form.Get("appointmentTypeID")))
                IsValid = false;

            IsValid = true;
        }

        private void Initalize(FormDataCollection form)
        {
            if (!IsValid) return;

            Action = form.Get("action");
            Id = form.Get("id");
            CalendarId = form.Get("calendarID");
            AppointmentTypeId = form.Get("appointmentTypeID");
            _asString = $"action={Action}&id={Id}&calendarID={CalendarId}&appointmentTypeID={AppointmentTypeId}";
        }

        #endregion

        #region Public Methods

        public string GetString()
        {
            if (IsValid) return _asString;

            throw new Exception(
                "Notification is not a valid instance of a notification.  The current values of this noticiation are:  " +
                $"action={Action}&id={Id}&calendarID={CalendarId}&appointmentTypeID={AppointmentTypeId}");
        }

        #endregion
    }

    public class NotificationConstants
    {
        public static readonly string Scheduled = "scheduled";
        public static readonly string Rescheduled = "rescheduled";
        public static readonly string Canceled = "canceled";
        public static readonly string Changed = "changed";
    }
}