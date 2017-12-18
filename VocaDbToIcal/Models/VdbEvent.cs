using System;

namespace VocaDbToIcal.Models {
	public class VdbEvent {

		public DateTime? Date { get; set; }

		public int Id { get; set; }

		public string Name { get; set; }

		public DateTime? EndDate { get; set; }

		public string UrlSlug { get; set; }

		public string VenueName { get; set; }

	}
}