using LouBuzReview.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LouBuzReview.Data
{
    //DropCreateDatabaseIfModelChanges
    //DropCreateDatabaseAlways
    public class LouBuzReviewInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<LouBuzReviewContext>
    {
        protected override void Seed(LouBuzReviewContext context)
        {
            var webUsers = new List<WebUser>
            {
                new WebUser{FirstName="Ann",    LastName="Smith",   City="Chicago",     State="IL"},
                new WebUser{FirstName="Andrew", LastName="Adams",   City="Columbus",    State="OH"},
                new WebUser{FirstName="Sam",    LastName="Summers", City="Duram",       State="NC"}
            };
            webUsers.ForEach(w => context.WebUsers.Add(w));
            context.SaveChanges();

            var website = new List<Website>
            {
                new Website{Category=Category.Dealership,         WebsiteUrl="https://www.oxmoortoyota.com/",   WebsiteName="Oxmoor Toyota"},
                new Website{Category=Category.Education,          WebsiteUrl="http://louisville.edu/",          WebsiteName="University of Louisville"},
                new Website{Category=Category.AutomobileServices, WebsiteUrl="https://smiky.com/",              WebsiteName="St. Matthews Imports"}
            };
            website.ForEach(w => context.Websites.Add(w));
            context.SaveChanges();

            var websiteReview = new List<WebsiteReview>
            {
                new WebsiteReview{UserID=1, WebsiteID=1, Ratings=Ratings.FiveStars,  UserReview="Love this place! So friendly and professional.What a great  experience and results. The agent was attentive and gave fantastic advice. If you are a car freak and want quality, go to St. Mathews.", CreatedDate=DateTime.Parse("2018-07-26")},
                new WebsiteReview{UserID=2, WebsiteID=2, Ratings=Ratings.TwoStars,   UserReview="If I could give this place zero stars, I would. Don't expect good customer service. My wife wanted to buy a Camry and walked in. Rep showed us where Camry cars parked and disappeared. Never showed up This is horrible customer service.", CreatedDate=DateTime.Parse("2018-07-26")},
                new WebsiteReview{UserID=3, WebsiteID=3, Ratings=Ratings.ThreeStars, UserReview="UofL is where I attended college for my last two years. My experiences there were mostly fun and educational. Some teachers were pretty good and there were some teachers there who had no right to teach because they either needed to retire or they were just awful teachers but I didn't run in to too much of a problem with this. The parking on UofLs campus is atrocious. You pay almost $200 for a parking pass and still have no where to park some days, or if you do find a parking spot you have to walk almost two miles to your class often making you late to class or worse unable to make it to class at all.", CreatedDate=DateTime.Parse("2018-07-26")}
            };
            websiteReview.ForEach(w => context.WebsiteReviews.Add(w));
            context.SaveChanges();
        }
    }
}
