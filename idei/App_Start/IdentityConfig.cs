using System.Linq;
using System.Security.Claims;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Web;
using idei.Models;

namespace IdentitySample.Models
{
    // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.

    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser> store)
            : base(store)
        {
        }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options,
            IOwinContext context)
        {
            var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(context.Get<ApplicationDbContext>()));
            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<ApplicationUser>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };
            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = true,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true,
            };
            // Configure user lockout defaults
            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;
            // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
            // You can write your own provider and plug in here.
            manager.RegisterTwoFactorProvider("PhoneCode", new PhoneNumberTokenProvider<ApplicationUser>
            {
                MessageFormat = "Your security code is: {0}"
            });
            manager.RegisterTwoFactorProvider("EmailCode", new EmailTokenProvider<ApplicationUser>
            {
                Subject = "SecurityCode",
                BodyFormat = "Your security code is {0}"
            });
            manager.EmailService = new EmailService();
            manager.SmsService = new SmsService();
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider =
                    new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }
    }

    // Configure the RoleManager used in the application. RoleManager is defined in the ASP.NET Identity core assembly
    public class ApplicationRoleManager : RoleManager<IdentityRole>
    {
        public ApplicationRoleManager(IRoleStore<IdentityRole,string> roleStore)
            : base(roleStore)
        {
        }

        public static ApplicationRoleManager Create(IdentityFactoryOptions<ApplicationRoleManager> options, IOwinContext context)
        {
            return new ApplicationRoleManager(new RoleStore<IdentityRole>(context.Get<ApplicationDbContext>()));
        }
    }

    public class EmailService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Plug in your email service here to send an email.
            var credentialUserName = "ideimusic@outlook.com";
            var sentFrom = "ideimusic@outlook.com";
            var pwd = "Qwerty123456";

            // Configure the client:
            System.Net.Mail.SmtpClient client =
                new System.Net.Mail.SmtpClient("smtp-mail.outlook.com");

            client.Port = 587;
            client.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;

            // Create the credentials:
            System.Net.NetworkCredential credentials =
                new System.Net.NetworkCredential(credentialUserName, pwd);

            client.EnableSsl = true;
            client.Credentials = credentials;

            // Create the message:
            var mail =
                new System.Net.Mail.MailMessage(sentFrom, message.Destination);

            mail.Subject = message.Subject;
            mail.Body = message.Body;

            // Send:
            return client.SendMailAsync(mail);
        }
    }

    public class SmsService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Plug in your sms service here to send a text message.
            return Task.FromResult(0);
        }
    }

    // This is useful if you do not want to tear down the database each time you run the application.
    // public class ApplicationDbInitializer : DropCreateDatabaseAlways<ApplicationDbContext>
    // This example shows you how to create a new database if the Model changes
    //public class ApplicationDbInitializer : DropCreateDatabaseIfModelChanges<ApplicationDbContext> 
    //{
    //    protected override void Seed(ApplicationDbContext context) {
    //        InitializeIdentityForEF(context);
    //        base.Seed(context);
    //    }

    //    //Create User=Admin@Admin.com with password=Admin@123456 in the Admin role        
    //    public static void InitializeIdentityForEF(ApplicationDbContext db) {
    //        var userManager = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
    //        var roleManager = HttpContext.Current.GetOwinContext().Get<ApplicationRoleManager>();
    //        const string name = "admin@example.com";
    //        const string password = "Admin@123456";
    //        const string roleName = "Admin";

    //        const string managerName = "manager@example.com";
    //        const string managerPassword = "Manager@123456";
    //        const string ManagerRoleName = "Manager";

    //        //Create Role Admin if it does not exist
    //        var role = roleManager.FindByName(roleName);
    //        if (role == null) {
    //            role = new IdentityRole(roleName);
    //            var roleresult = roleManager.Create(role);
    //        }

    //        var managerRole = roleManager.FindByName(ManagerRoleName);
    //        if (managerRole == null)
    //        {
    //            managerRole = new IdentityRole(ManagerRoleName);
    //            var managerRoleResult = roleManager.Create(managerRole);
    //        }

    //        var user = userManager.FindByName(name);
    //        if (user == null) {
    //            user = new ApplicationUser { UserName = name, Email = name, EmailConfirmed = true };
    //            var result = userManager.Create(user, password);
    //            result = userManager.SetLockoutEnabled(user.Id, false);
    //        }

    //        var managerUser = userManager.FindByName(managerName);
    //        if (managerUser == null)
    //        {
    //            managerUser = new ApplicationUser { UserName = managerName, Email = managerName, EmailConfirmed = true };
    //            userManager.Create(managerUser, managerPassword);
    //            userManager.SetLockoutEnabled(managerUser.Id, false);
    //        }

    //        var temp = new ApplicationUser { UserName = "tmp@hue.br", Email = "tmp@hue.br" };
    //        userManager.Create(temp, "Tmp@123");

    //        // Add user admin to Role Admin if not already added
    //        var rolesForUser = userManager.GetRoles(user.Id);
    //        if (!rolesForUser.Contains(role.Name)) {
    //            var result = userManager.AddToRole(user.Id, role.Name);
    //        }
         
    //        var rolesForManagerUser = userManager.GetRoles(managerUser.Id);
    //        if (!rolesForManagerUser.Contains(managerRole.Name))
    //        {
    //            userManager.AddToRole(managerUser.Id, managerRole.Name);
    //        }

    //        var formats = new List<Format>
    //        {
    //            new Format { Name = "CD"},
    //            new Format { Name = "Vinyl"},
    //        };
    //        formats.ForEach(f => db.Formats.Add(f));
    //        db.SaveChanges();

    //        var genres = new List<Genre>
    //        {
    //            new Genre { Name = "Rock" },
    //            new Genre { Name = "Jazz" },
    //            new Genre { Name = "Metal" },
    //            new Genre { Name = "Alternative" },
    //            new Genre { Name = "Disco" },
    //            new Genre { Name = "Blues" },
    //            new Genre { Name = "Latin" },
    //            new Genre { Name = "Reggae" },
    //            new Genre { Name = "Pop" },
    //            new Genre { Name = "Classical" }
    //        };
    //        genres.ForEach(g => db.Genres.Add(g));
    //        db.SaveChanges();

    //        var artists = new List<Artist>
    //        {
    //            new Artist { Name = "Aaron Copland & London Symphony Orchestra" },
    //            new Artist { Name = "Aaron Goldberg" },
    //            new Artist { Name = "AC/DC" },
    //            new Artist { Name = "Accept" },
    //            new Artist { Name = "Adrian Leaper & Doreen de Feis" },
    //            new Artist { Name = "Aerosmith" },
    //            new Artist { Name = "Aisha Duo" },
    //            new Artist { Name = "Alanis Morissette" },
    //            new Artist { Name = "Alberto Turco & Nova Schola Gregoriana" },
    //            new Artist { Name = "Alice In Chains" },
    //            new Artist { Name = "Amy Winehouse" },
    //            new Artist { Name = "Anita Ward" },
    //            new Artist { Name = "Antônio Carlos Jobim" },
    //            new Artist { Name = "Apocalyptica" },
    //            new Artist { Name = "Audioslave" },
    //            new Artist { Name = "Barry Wordsworth & BBC Concert Orchestra" },
    //            new Artist { Name = "Berliner Philharmoniker & Hans Rosbaud" },
    //            new Artist { Name = "Berliner Philharmoniker & Herbert Von Karajan" },
    //            new Artist { Name = "Billy Cobham" },
    //            new Artist { Name = "Black Label Society" },
    //            new Artist { Name = "Black Sabbath" },
    //            new Artist { Name = "Boston Symphony Orchestra & Seiji Ozawa" },
    //            new Artist { Name = "Britten Sinfonia, Ivor Bolton & Lesley Garrett" },
    //            new Artist { Name = "Bruce Dickinson" },
    //            new Artist { Name = "Buddy Guy" },
    //            new Artist { Name = "Caetano Veloso" },
    //            new Artist { Name = "Cake" },
    //            new Artist { Name = "Calexico" },
    //            new Artist { Name = "Cássia Eller" },
    //            new Artist { Name = "Chic" },
    //            new Artist { Name = "Chicago Symphony Orchestra & Fritz Reiner" },
    //            new Artist { Name = "Chico Buarque" },
    //            new Artist { Name = "Chico Science & Nação Zumbi" },
    //            new Artist { Name = "Choir Of Westminster Abbey & Simon Preston" },
    //            new Artist { Name = "Chris Cornell" },
    //            new Artist { Name = "Christopher O'Riley" },
    //            new Artist { Name = "Cidade Negra" },
    //            new Artist { Name = "Cláudio Zoli" },
    //            new Artist { Name = "Creedence Clearwater Revival" },
    //            new Artist { Name = "David Coverdale" },
    //            new Artist { Name = "Deep Purple" },
    //            new Artist { Name = "Dennis Chambers" },
    //            new Artist { Name = "Djavan" },
    //            new Artist { Name = "Donna Summer" },
    //            new Artist { Name = "Dread Zeppelin" },
    //            new Artist { Name = "Ed Motta" },
    //            new Artist { Name = "Edo de Waart & San Francisco Symphony" },
    //            new Artist { Name = "Elis Regina" },
    //            new Artist { Name = "English Concert & Trevor Pinnock" },
    //            new Artist { Name = "Eric Clapton" },
    //            new Artist { Name = "Eugene Ormandy" },
    //            new Artist { Name = "Faith No More" },
    //            new Artist { Name = "Falamansa" },
    //            new Artist { Name = "Foo Fighters" },
    //            new Artist { Name = "Frank Zappa & Captain Beefheart" },
    //            new Artist { Name = "Fretwork" },
    //            new Artist { Name = "Funk Como Le Gusta" },
    //            new Artist { Name = "Gerald Moore" },
    //            new Artist { Name = "Gilberto Gil" },
    //            new Artist { Name = "Godsmack" },
    //            new Artist { Name = "Gonzaguinha" },
    //            new Artist { Name = "Göteborgs Symfoniker & Neeme Järvi" },
    //            new Artist { Name = "Guns N' Roses" },
    //            new Artist { Name = "Gustav Mahler" },
    //            new Artist { Name = "Incognito" },
    //            new Artist { Name = "Iron Maiden" },
    //            new Artist { Name = "James Levine" },
    //            new Artist { Name = "Jamiroquai" },
    //            new Artist { Name = "Jimi Hendrix" },
    //            new Artist { Name = "Joe Satriani" },
    //            new Artist { Name = "Jorge Ben" },
    //            new Artist { Name = "Jota Quest" },
    //            new Artist { Name = "Judas Priest" },
    //            new Artist { Name = "Julian Bream" },
    //            new Artist { Name = "Kent Nagano and Orchestre de l'Opéra de Lyon" },
    //            new Artist { Name = "Kiss" },
    //            new Artist { Name = "Led Zeppelin" },
    //            new Artist { Name = "Legião Urbana" },
    //            new Artist { Name = "Lenny Kravitz" },
    //            new Artist { Name = "Les Arts Florissants & William Christie" },
    //            new Artist { Name = "London Symphony Orchestra & Sir Charles Mackerras" },
    //            new Artist { Name = "Luciana Souza/Romero Lubambo" },
    //            new Artist { Name = "Lulu Santos" },
    //            new Artist { Name = "Marcos Valle" },
    //            new Artist { Name = "Marillion" },
    //            new Artist { Name = "Marisa Monte" },
    //            new Artist { Name = "Martin Roscoe" },
    //            new Artist { Name = "Maurizio Pollini" },
    //            new Artist { Name = "Mela Tenenbaum, Pro Musica Prague & Richard Kapp" },
    //            new Artist { Name = "Men At Work" },
    //            new Artist { Name = "Metallica" },
    //            new Artist { Name = "Michael Tilson Thomas & San Francisco Symphony" },
    //            new Artist { Name = "Miles Davis" },
    //            new Artist { Name = "Milton Nascimento" },
    //            new Artist { Name = "Mötley Crüe" },
    //            new Artist { Name = "Motörhead" },
    //            new Artist { Name = "Nash Ensemble" },
    //            new Artist { Name = "Nicolaus Esterhazy Sinfonia" },
    //            new Artist { Name = "Nirvana" },
    //            new Artist { Name = "O Terço" },
    //            new Artist { Name = "Olodum" },
    //            new Artist { Name = "Orchestra of The Age of Enlightenment" },
    //            new Artist { Name = "Os Paralamas Do Sucesso" },
    //            new Artist { Name = "Ozzy Osbourne" },
    //            new Artist { Name = "Page & Plant" },
    //            new Artist { Name = "Paul D'Ianno" },
    //            new Artist { Name = "Pearl Jam" },
    //            new Artist { Name = "Pink Floyd" },
    //            new Artist { Name = "Queen" },
    //            new Artist { Name = "R.E.M." },
    //            new Artist { Name = "Raul Seixas" },
    //            new Artist { Name = "Red Hot Chili Peppers" },
    //            new Artist { Name = "Roger Norrington, London Classical Players" },
    //            new Artist { Name = "Royal Philharmonic Orchestra & Sir Thomas Beecham" },
    //            new Artist { Name = "Rush" },
    //            new Artist { Name = "Santana" },
    //            new Artist { Name = "Scholars Baroque Ensemble" },
    //            new Artist { Name = "Scorpions" },
    //            new Artist { Name = "Sergei Prokofiev & Yuri Temirkanov" },
    //            new Artist { Name = "Sir Georg Solti & Wiener Philharmoniker" },
    //            new Artist { Name = "Skank" },
    //            new Artist { Name = "Soundgarden" },
    //            new Artist { Name = "Spyro Gyra" },
    //            new Artist { Name = "Stevie Ray Vaughan & Double Trouble" },
    //            new Artist { Name = "Stone Temple Pilots" },
    //            new Artist { Name = "System Of A Down" },
    //            new Artist { Name = "Temple of the Dog" },
    //            new Artist { Name = "Terry Bozzio, Tony Levin & Steve Stevens" },
    //            new Artist { Name = "The 12 Cellists of The Berlin Philharmonic" },
    //            new Artist { Name = "The Black Crowes" },
    //            new Artist { Name = "The Cult" },
    //            new Artist { Name = "The Doors" },
    //            new Artist { Name = "The King's Singers" },
    //            new Artist { Name = "The Police" },
    //            new Artist { Name = "The Posies" },
    //            new Artist { Name = "The Rolling Stones" },
    //            new Artist { Name = "The Who" },
    //            new Artist { Name = "Tim Maia" },
    //            new Artist { Name = "Ton Koopman" },
    //            new Artist { Name = "U2" },
    //            new Artist { Name = "UB40" },
    //            new Artist { Name = "Van Halen" },
    //            new Artist { Name = "Various Artists" },
    //            new Artist { Name = "Velvet Revolver" },
    //            new Artist { Name = "Vinícius De Moraes" },
    //            new Artist { Name = "Wilhelm Kempff" },
    //            new Artist { Name = "Yehudi Menuhin" },
    //            new Artist { Name = "Yo-Yo Ma" },
    //            new Artist { Name = "Zeca Pagodinho" }
    //        };
    //        artists.ForEach(a => db.Artists.Add(a));
    //        db.SaveChanges();

    //        var records = new List<Record>
    //        {
    //            new Record { Title = "A Copland Celebration, Vol. I",ShopSales = 10, Format = formats.Single(f => f.Name == "CD"), Genre = genres.Single(g => g.Name == "Classical"), Price = 8.99M, Artist = artists.Single(a => a.Name == "Aaron Copland & London Symphony Orchestra"), AlbumArtUrl = "/Content/Images/placeholder.gif" },
    //            new Record { Title = "Worlds",ShopSales = 20, Format = formats.Single(f => f.Name == "CD"), Genre = genres.Single(g => g.Name == "Jazz"), Price = 8.99M, Artist = artists.Single(a => a.Name == "Aaron Goldberg"), AlbumArtUrl = "/Content/Images/placeholder.gif" },
    //            new Record { Title = "For Those About To Rock We Salute You",ShopSales = 1, Format = formats.Single(f => f.Name == "CD"), Genre = genres.Single(g => g.Name == "Rock"), Price = 8.99M, Artist = artists.Single(a => a.Name == "AC/DC"), AlbumArtUrl = "/Content/Images/placeholder.gif" },
    //            new Record { Title = "Let There Be Rock",ShopSales = 30, Format = formats.Single(f => f.Name == "Vinyl"), Genre = genres.Single(g => g.Name == "Rock"), Price = 8.99M, Artist = artists.Single(a => a.Name == "AC/DC"), AlbumArtUrl = "/Content/Images/placeholder.gif" },
    //            new Record { Title = "Balls to the Wall",ShopSales = 400, Format = formats.Single(f => f.Name == "Vinyl"), Genre = genres.Single(g => g.Name == "Rock"), Price = 8.99M, Artist = artists.Single(a => a.Name == "Accept"), AlbumArtUrl = "/Content/Images/placeholder.gif" },
    //            new Record { Title = "Restless and Wild",ShopSales = 500, Format = formats.Single(f => f.Name == "Vinyl"), Genre = genres.Single(g => g.Name == "Rock"), Price = 8.99M, Artist = artists.Single(a => a.Name == "Accept"), AlbumArtUrl = "/Content/Images/placeholder.gif" },
              
    //        };
    //        records.ForEach(r => db.Records.Add(r));
    //        db.SaveChanges();

         
    //        //var userManager = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
    //        var orders = new List<Order>{ 
                             
    //            new Order{ 
    //                User = userManager.FindByEmail( "admin@example.com" ), OrderDate = new DateTime(2014,12,24)/*,Total=150*/},
    //                new Order{ 
    //                User = userManager.FindByEmail( "manager@example.com" ), OrderDate = new DateTime(2014,12,25),/*Total=50*/}
    //        };
    //        orders.ForEach(o => db.Orders.Add(o));
    //        db.SaveChanges();


    //        var orderLists = new List<OrderList>{
    //            new OrderList{Order = orders[0],Quantity = 2,UnitPrice = 50, Record = records.Single(r => r.Title == "Balls to the Wall" )},
    //            new OrderList{Order = orders[1],Quantity = 4,UnitPrice = 50, Record = records.Single(r => r.Title == "Balls to the Wall" )}
    //        };
    //        orderLists.ForEach(o => db.OrderLists.Add(o));
    //        db.SaveChanges();

    //        Record test = db.Records.Single(o => o.Title == "Balls to the Wall");
    //        test.ShopSales += 1;
    //        db.SaveChanges();
    //    }
    //}
       
    

    public class ApplicationSignInManager : SignInManager<ApplicationUser, string>
    {
        public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager) : 
            base(userManager, authenticationManager) { }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(ApplicationUser user)
        {
            return user.GenerateUserIdentityAsync((ApplicationUserManager)UserManager);
        }

        public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options, IOwinContext context)
        {
            return new ApplicationSignInManager(context.GetUserManager<ApplicationUserManager>(), context.Authentication);
        }
    }
}