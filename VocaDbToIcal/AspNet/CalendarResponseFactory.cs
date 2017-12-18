using System.Collections.Generic;
using Ical.Net;
using Ical.Net.CalendarComponents;
using Ical.Net.Serialization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace VocaDbToIcal.AspNet {

	public class CalendarResponseFactory {

		public static ContentResult CreateCalendarContentResult(HttpResponse response, IEnumerable<CalendarEvent> events, string filename) {

			var calendar = new Calendar();
			calendar.Events.AddRange(events);
			var serializer = new CalendarSerializer();
			var serializedCalendar = serializer.SerializeToString(calendar);

			response.Headers.Add("Content-Disposition", "attachment; filename=\"" + filename + "\"");
			return new ContentResult { Content = serializedCalendar, ContentType = "text/calendar" };

		}

	}
}
