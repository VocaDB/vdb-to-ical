using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Ical.Net;
using Ical.Net.CalendarComponents;
using Ical.Net.DataTypes;
using Ical.Net.Serialization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace VocaDbToIcal.Controllers {

	[Route("")]
	public class EventsController : Controller {

		private CalendarEvent CreateCalendarEvent(VdbEvent vdbEvent) {

			var e = new CalendarEvent {
				Start = new CalDateTime(vdbEvent.Date.Value),
				End = new CalDateTime(vdbEvent.EndDate ?? vdbEvent.Date.Value),
				IsAllDay = true,
				Summary = vdbEvent.Name,
				Location = vdbEvent.VenueName,
				Url = new Uri(string.Format("http://vocadb.net/E/{0}/{1}", vdbEvent.Id, vdbEvent.UrlSlug))
			};

			return e;

		}

		[HttpGet]
		[Route("")]
		[Route("events")]
		public async Task<ContentResult> GetIcal() {
			
			var client = new HttpClient();
			var start = DateTime.Now.AddDays(-1);
			var end = start.AddDays(60);
			var url = string.Format("https://vocadb.net/api/releaseEvents?sort=Date&maxResults=100&afterDate={0:u}&beforeDate={1:u}", start, end);
			var result = await client.GetAsync(url);

			result.EnsureSuccessStatusCode();

			var str = await result.Content.ReadAsStringAsync();
			var vbEvents = JsonConvert.DeserializeObject<PartialFindResult<VdbEvent>>(str);
			var events = vbEvents.Items.Where(e => e.Date.HasValue).Select(CreateCalendarEvent);
			var calendar = new Calendar { Name = "VocaDB events" };
			calendar.Events.AddRange(events);

			var serializer = new CalendarSerializer();
			var serializedCalendar = serializer.SerializeToString(calendar);

			Request.HttpContext.Response.Headers.Add("Content-Disposition", "attachment; filename=\"events.ics\"");
			return new ContentResult { Content = serializedCalendar, ContentType = "text/calendar" };

		}

	}

	public class PartialFindResult<T> {
		public T[] Items { get; set; }
	}

	public class VdbEvent {

		public DateTime? Date { get; set; }

		public int Id { get; set; }

		public string Name { get; set; }

		public DateTime? EndDate { get; set; }

		public string UrlSlug { get; set; }

		public string VenueName { get; set; }

	}

}
