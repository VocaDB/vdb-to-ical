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

	[Route("events")]
	public class EventsController : Controller {

		private CalendarEvent CreateCalendarEvent(VdbEvent vdbEvent) {

			var e = new CalendarEvent {
				Start = new CalDateTime(vdbEvent.Date.Value),
				End = new CalDateTime(vdbEvent.EndDate ?? vdbEvent.Date.Value),
				IsAllDay = true,
				Summary = vdbEvent.Name
			};

			return e;

		}

		[HttpGet("")]
		public async Task<ContentResult> GetIcal() {
			
			var client = new HttpClient();
			var start = DateTime.Now.AddDays(-1);
			var end = start.AddDays(8);
			var url = string.Format("https://vocadb.net/api/releaseEvents?sort=Date&afterDate={0:u}&beforeDate={1:u}", start, end);
			var result = await client.GetAsync(url);

			result.EnsureSuccessStatusCode();

			var str = await result.Content.ReadAsStringAsync();
			var vbEvents = JsonConvert.DeserializeObject<PartialFindResult<VdbEvent>>(str);
			var events = vbEvents.Items.Where(e => e.Date.HasValue).Select(CreateCalendarEvent);
			var calendar = new Calendar();
			calendar.Events.AddRange(events);

			var serializer = new CalendarSerializer();
			var serializedCalendar = serializer.SerializeToString(calendar);

			return new ContentResult {  Content = serializedCalendar, ContentType = "text/calendar" };

		}

	}

	public class PartialFindResult<T> {
		public T[] Items { get; set; }
	}

	public class VdbEvent {

		public DateTime? Date { get; set; }

		public string Name { get; set; }

		public DateTime? EndDate { get; set; }

	}

}
