namespace VocaDbToIcal.Models {

	public class Album {

		public string ArtistString { get; set; }

		public int Id { get; set; }

		public string Name { get; set; }

		public OptionalDateTimeContract ReleaseDate { get; set; }

	}

}
