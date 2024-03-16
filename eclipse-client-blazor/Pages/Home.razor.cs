using System.Globalization;
using System.Net;
using PALib;

namespace eclipse_client_blazor.Pages
{
    public partial class Home
    {
        private double latitude;
        private double longitude;
        private bool isDaylightSavings;
        private int zoneCorrectionHours;
        private DateTime calcDate;

        private Occurrence occurrence;
        private Circumstances circumstances;

        public Home()
        {
            calcDate = new DateTime(2024, 4, 8);

            occurrence = new() { Status = "" };
            circumstances = new() { FormattedFirstContact = "" };

            InitLocation();
        }

        private void InitLocation()
        {
            latitude = 39.74722;
            longitude = -84.53639;

            isDaylightSavings = true;
            zoneCorrectionHours = 5;
        }

        public void Calculate()
        {
            PAEclipses paEclipses = new();

            (string status, double eventDateDay, int eventDateMonth, int eventDateYear) = paEclipses.SolarEclipseOccurrence(calcDate.Day, calcDate.Month, calcDate.Year, isDaylightSavings, zoneCorrectionHours);

            occurrence.Status = status;
            occurrence.EventDateDay = eventDateDay;
            occurrence.EventDateMonth = eventDateMonth;
            occurrence.EventDateYear = eventDateYear;

            (double solarEclipseCertainDateDay, int solarEclipseCertainDateMonth, int solarEclipseCertainDateYear, double utFirstContactHour, double utFirstContactMinutes, double utMidEclipseHour, double utMidEclipseMinutes, double utLastContactHour, double utLastContactMinutes, double eclipseMagnitude) = paEclipses.SolarEclipseCircumstances(calcDate.Day, calcDate.Month, calcDate.Year, isDaylightSavings, zoneCorrectionHours, longitude, latitude);

            circumstances.SolarEclipseCertainDateDay = solarEclipseCertainDateDay;
            circumstances.SolarEclipseCertainDateMonth = solarEclipseCertainDateMonth;
            circumstances.SolarEclipseCertainDateYear = solarEclipseCertainDateYear;

            circumstances.UtFirstContactHour = utFirstContactHour - (isDaylightSavings ? zoneCorrectionHours - 1 : zoneCorrectionHours);
            circumstances.UtFirstContactMinutes = utFirstContactMinutes;

            circumstances.UtMidEclipseHour = utMidEclipseHour - (isDaylightSavings ? zoneCorrectionHours - 1 : zoneCorrectionHours);
            circumstances.UtMidEclipseMinutes = utMidEclipseMinutes;

            circumstances.UtLastContactHour = utLastContactHour - (isDaylightSavings ? zoneCorrectionHours - 1 : zoneCorrectionHours);
            circumstances.UtLastContactMinutes = utLastContactMinutes;

            circumstances.EclipseMagnitude = eclipseMagnitude;
        }
    }

    public class Occurrence
    {
        private string? status;
        private double eventDateDay;
        private int eventDateMonth;
        private int eventDateYear;

        public string? Status { get => status; set => status = value; }
        public double EventDateDay { get => eventDateDay; set => eventDateDay = value; }
        public int EventDateMonth { get => eventDateMonth; set => eventDateMonth = value; }
        public int EventDateYear { get => eventDateYear; set => eventDateYear = value; }
    }

    public class Circumstances
    {
        private double solarEclipseCertainDateDay;
        private int solarEclipseCertainDateMonth;
        private int solarEclipseCertainDateYear;
        private double utFirstContactHour;
        private double utFirstContactMinutes;
        private double utMidEclipseHour;
        private double utMidEclipseMinutes;
        private double utLastContactHour;
        private double utLastContactMinutes;
        private double eclipseMagnitude;
        private string? formattedFirstContact;
        private string? formattedMidEclipse;
        private string? formattedLastContact;
        private string? formattedEclipseMagnitude;

        public double SolarEclipseCertainDateDay { get => solarEclipseCertainDateDay; set => solarEclipseCertainDateDay = value; }
        public int SolarEclipseCertainDateMonth { get => solarEclipseCertainDateMonth; set => solarEclipseCertainDateMonth = value; }
        public int SolarEclipseCertainDateYear { get => solarEclipseCertainDateYear; set => solarEclipseCertainDateYear = value; }
        public double UtFirstContactHour
        {
            get => utFirstContactHour;

            set
            {
                utFirstContactHour = value;

                UpdateFormattedFirstContact();
            }
        }
        public double UtFirstContactMinutes
        {
            get => utFirstContactMinutes;

            set
            {
                utFirstContactMinutes = value;

                UpdateFormattedFirstContact();
            }
        }
        public string? FormattedFirstContact { get => formattedFirstContact; set => formattedFirstContact = value; }
        public double UtMidEclipseHour
        {
            get => utMidEclipseHour;

            set
            {
                utMidEclipseHour = value;

                UpdateFormattedMidEclipse();
            }
        }
        public double UtMidEclipseMinutes
        {
            get => utMidEclipseMinutes;

            set
            {
                utMidEclipseMinutes = value;

                UpdateFormattedMidEclipse();
            }
        }
        public string? FormattedMidEclipse { get => formattedMidEclipse; set => formattedMidEclipse = value; }
        public double UtLastContactHour
        {
            get => utLastContactHour;
            set
            {
                utLastContactHour = value;

                UpdateFormattedLastContact();
            }
        }

        public double UtLastContactMinutes
        {
            get => utLastContactMinutes;
            set
            {
                utLastContactMinutes = value;

                UpdateFormattedLastContact();
            }
        }

        public string? FormattedLastContact { get => formattedLastContact; set => formattedLastContact = value; }

        public double EclipseMagnitude
        {
            get => eclipseMagnitude;
            set
            {
                eclipseMagnitude = value;

                formattedEclipseMagnitude = (eclipseMagnitude >= 1) ? "100 %" : $"{eclipseMagnitude * 100} %";
            }
        }

        public string? FormattedEclipseMagnitude { get => formattedEclipseMagnitude; set => formattedEclipseMagnitude = value; }

        private void UpdateFormattedFirstContact()
        {
            DateTime time = new DateTime(solarEclipseCertainDateYear, solarEclipseCertainDateMonth, (int)solarEclipseCertainDateDay, (int)utFirstContactHour, (int)utFirstContactMinutes, 0);

            formattedFirstContact = time.ToString("h:mm tt", CultureInfo.InvariantCulture);
        }

        private void UpdateFormattedMidEclipse()
        {
            DateTime time = new DateTime(solarEclipseCertainDateYear, solarEclipseCertainDateMonth, (int)solarEclipseCertainDateDay, (int)utMidEclipseHour, (int)utMidEclipseMinutes, 0);

            formattedMidEclipse = time.ToString("h:mm tt", CultureInfo.InvariantCulture);
        }

        private void UpdateFormattedLastContact()
        {
            DateTime time = new DateTime(solarEclipseCertainDateYear, solarEclipseCertainDateMonth, (int)solarEclipseCertainDateDay, (int)utLastContactHour, (int)utLastContactMinutes, 0);

            formattedLastContact = time.ToString("h:mm tt", CultureInfo.InvariantCulture);
        }
    }
}