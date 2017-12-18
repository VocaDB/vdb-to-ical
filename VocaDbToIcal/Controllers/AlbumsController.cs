using System;
using System.Linq;
using System.Threading.Tasks;
using Ical.Net.CalendarComponents;
using Ical.Net.DataTypes;
using Microsoft.AspNetCore.Mvc;
using VocaDbToIcal.AspNet;
using VocaDbToIcal.Models;

namespace VocaDbToIcal.Controllers {

	[Route("albums")]
	public class AlbumsController : Controller {

		private CalendarEvent CreateCalendarEvent(Album album) {

			var e = new CalendarEvent {
				Start = new CalDateTime(album.ReleaseDate.Year.Value, album.ReleaseDate.Month ?? 1, album.ReleaseDate.Day ?? 1),
				IsAllDay = true,
				Summary = album.Name.Truncate(70),
				Description = album.ArtistString.Truncate(60),
				Url = new Uri(string.Format("http://vocadb.net/Al/{0}", album.Id))
			};

			return e;

		}

		[HttpGet("")]
		public async Task<ContentResult> GetAlbums() {

			var start = DateTime.Now.AddDays(-1);
			var end = start.AddDays(60);
			var url = string.Format("https://vocadb.net/api/albums?sort=ReleaseDate&maxResults=100&releaseDateAfter={0:u}&releaseDateBefore={1:u}", start, end);

			var albums = await JsonHttpClient.GetObject<PartialFindResult<Album>>(url);
			var events = albums.Items.Select(CreateCalendarEvent);

			return CalendarResponseFactory.CreateCalendarContentResult(Response, events, "albums.ics");

		}

	}

}
