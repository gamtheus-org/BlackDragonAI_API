// using System;
// using System.Threading.Tasks;
// using BlackDragonAIAPI.Discord;
// using BlackDragonAIAPI.Models;
// using Microsoft.Extensions.Options;
// using NUnit.Framework;
//
// namespace Tests
// {
//     public class DiscordManagerTests
//     {
//         private DiscordManager _discordManager;
//         
//         [SetUp]
//         public async Task SetUp()
//         {
//             this._discordManager = new DiscordManager();
//             await this._discordManager.Connect();
//         }
//         
//         [Test]
//         public async Task TestRead()
//         {
//             // Arrange
//             
//             // Act
//             var message = await this._discordManager.GetMessage();
//
//             // Assert
//             var expectedMessage = @"<:bkdnPOG:747104206208499812>  Jaarplanning  <:bkdnLurk:747104214731325490> 
//
//
// Do. 19-08  |  Avond  |  Twelve Minutes  |  Let's Play
// Thriller / Story  |  Trailer: https://youtu.be/izoUXOMyVyk
//
// Vr. 20-08  |  Avond  |  Ghost of Tsushima Director's Cut  |  DLC Let's Play
// Samurai / Open World  |  Trailer: https://youtu.be/A5gVt028Hww
//
// Ma. 23-08  |  Avond  |  Riders Republic  |  Beta met Daryll en Damian
// Open World / Sport  |  Trailer: https://youtu.be/ZPG1UIJjcks
//
// Wo. 25-08  |  Avond  |  Psychonauts 2  |  Showcase of Let's Play
// Action Adventure / Story  |  Trailer: https://youtu.be/ghO7GcYfrjc
//
// Vr. 27-08  |  Avond  |  Baldo The Guardian Owls  |  Let's Play
// Action Adventure / Open World  |  Trailer: https://youtu.be/7EgK5yLR37E
//
// Di. 31-08  |  Avond  |  KeyWe  |  Let's Play met Damian
// Coop / Puzzle  |  Trailer: https://youtu.be/54q_S-x8CCY
//
// Vr. 10-09  |  Avond  |  Life is Strange True Colors  |  Let's Play
// Story / Graphic Adventure  |  Trailer: https://youtu.be/b6CkzwVAr0M
//
// Zo. 12-09  |  Avond  |  Lost in Random  |  Showcase
// Action Adventure / Indie  |  Trailer: https://youtu.be/diilMn5gSAg
//
// Di. 14-09  |  Avond  |  Deathloop  |  Showcase
// Shooter / Action  |  Trailer: https://youtu.be/mc2hz3LJhTY
//
// Di. 21-09  |  Nacht & Avond  |  Kena: Bridge of Spirits  |  Let's Play
// Action Adventure / Story  |  Trailer: https://youtu.be/pWh5388AEHw
//
// Vr. 24-09  |  Avond  |  Death Stranding - Director's Cut  |  DLC Showcase
// Story / Open World  |  Trailer: https://youtu.be/6tgsz7WbidU
//
// Ma. 27-09  |  Avond  |  Hot Wheels Unleashed  |  Showcase
// Racing / Track Builder  |  Trailer: https://youtu.be/FDuYs0AhHbE
//
// Do. 07-10  |  Nacht & Avond  |  Far Cry 6  |  Showcase of Let's Play
// Action Adventure / Open World  |  Trailer: https://youtu.be/VbF6REQyel4
//
// Vr. 15-10  |  Nacht, Middag & Avond  |  Battlefield 2042  |  Knallen met Daryll en Damian
// Shooter / All-out Warfare  |  Trailer: https://youtu.be/WomAGoEh-Ss";
//             Assert.AreEqual(expectedMessage.Replace("\r", string.Empty), message.Content);
//         }
//
//         [Test]
//         public async Task WriteTest()
//         {
//             // Arrange
//             var sp1 = new StreamPlanning()
//             {
//                 Date = new DateTime(2021,10, 26),
//                 TimeSlot = "Avond",
//                 Game = "Marvel’s Guardians of the Galaxy",
//                 StreamType = "Showcase of Let's Play",
//                 GameType = "Action Adventure / Story",
//                 TrailerUri = "https://youtu.be/QBn8ST8rELc"
//             };
//             var sp2 = new StreamPlanning()
//             {
//                 Date = new DateTime(2021, 11, 5),
//                 TimeSlot = "Nacht, Middag & Avond",
//                 Game = "Forza Horizon 5",
//                 StreamType = "Racen met Daryll en Damian",
//                 GameType = "Racing / Open World",
//                 TrailerUri = "https://youtu.be/FYH9n37B7Yw"
//             };
//             var sp3 = new StreamPlanning()
//             {
//                 Date = new DateTime(2021, 12, 7),
//                 TimeSlot = "Nacht & Avond",
//                 Game = "Dying Light 2",
//                 StreamType = "Showcase of Let's Play",
//                 GameType = "Zombie / Open World",
//                 TrailerUri = "https://youtu.be/UwJAAy7tPhE"
//             };
//             
//             // Act
//             await this._discordManager.WriteStreamPlanning(new[] { sp1, sp2, sp3 });
//
//             // Assert
//         }
//
//         [Test]
//         public async Task SetUpTest()
//         {
//             // Arrange
//             
//             // Act
//             await this._discordManager.SetUp();
//
//             // Assert
//
//         }
//     }
// }