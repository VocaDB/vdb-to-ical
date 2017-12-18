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
using VocaDbToIcal.AspNet;
using VocaDbToIcal.Models;

namespace VocaDbToIcal.Controllers {

	[Route("events")]
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

		[HttpGet("")]
		public async Task<ContentResult> GetEvents() {
			
			var start = DateTime.Now.AddDays(-1);
			var end = start.AddDays(60);
			var url = string.Format("https://vocadb.net/api/releaseEvents?sort=Date&maxResults=100&afterDate={0:u}&beforeDate={1:u}", start, end);

			var vbEvents = await JsonHttpClient.GetObject<PartialFindResult<VdbEvent>>(url);
			var events = vbEvents.Items.Where(e => e.Date.HasValue).Select(CreateCalendarEvent);

			return CalendarResponseFactory.CreateCalendarContentResult(Response, events, "events.ics");

		}

	}

}
