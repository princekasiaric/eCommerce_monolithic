using System;
using System.IO;
using System.Text.Json;
using Construmart.Core.Domain.Models;
using Construmart.Core.DTOs.Response;
using Microsoft.EntityFrameworkCore;

namespace Construmart.Infrastructure.Data.EfCore.DataSeeders
{
    public static class NigerianStateSeeder
    {
        public static void SeedNigerianState(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<NigerianState>(builder =>
            {
                Console.WriteLine($"Current Environment Dir: {Environment.CurrentDirectory}");
                string path = Path.Combine(Environment.CurrentDirectory, "nigerian-lgas.json"); //on container: /app/nigerian-lgas.json
                Console.WriteLine($"Json Data Path: {path}");
                string jsonData = File.ReadAllText(path);
                var nigerianStates = JsonSerializer.Deserialize<NigerianStateResponse[]>(jsonData, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                for (int i = 0; i < nigerianStates.Length; i++)
                {
                    var state = nigerianStates[i];
                    var id = i + 1;
                    var nigerianState = NigerianState.Create(id, state.State, state.LocalGovernmentAreas);
                    builder.HasData(nigerianState);
                }
            });
        }
    }
}