using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Labb3_Quiz.Models;
using System.Text.Json;
using System.IO;
using System.Windows;

namespace Labb3_Quiz.Services
{
    internal class DataService
    {
        private readonly string _filePath;

        public DataService()
        {
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string _folderPath = Path.Combine(appDataPath, "Labb3_Quiz");

            try
            {
                if (!Directory.Exists(_folderPath))
                {
                    Directory.CreateDirectory(_folderPath);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error creating folder: {ex.Message}");
            }

            _filePath = Path.Combine(_folderPath, "packs.json");
        }

        public async Task<List<QuestionPack>> LoadPacksAsync()
        {
            try
            {
                if (!File.Exists(_filePath))
                {
                    return new List<QuestionPack>();
                }

                using FileStream stream = File.OpenRead(_filePath);
                var options = new JsonSerializerOptions { AllowTrailingCommas = true };

                var packs = await JsonSerializer.DeserializeAsync<List<QuestionPack>>(stream, options);
                return packs ?? new List<QuestionPack>();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading packs: {ex.Message}");
                return new List<QuestionPack>();
            }
        }

        public async Task SaveOrUpdatePacksAsync(QuestionPack pack)
        {
            try
            {
                var packs = await LoadPacksAsync();

                var existing = packs.FirstOrDefault(p => p.Name == pack.Name);

                if (existing != null)
                {
                    existing.Difficulty = pack.Difficulty;
                    existing.Name = pack.Name;
                    existing.Questions = pack.Questions;
                }
                else
                {
                    packs.Add(pack);
                }

                var options = new JsonSerializerOptions { WriteIndented = true };
                using FileStream stream = File.Create(_filePath);
                await JsonSerializer.SerializeAsync(stream, packs, options);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving packs: {ex.Message}");
            }
        }

        public async Task DeletePackAsync(string packName)
        {
            try
            {
                var packs = await LoadPacksAsync();

                var toRemove = packs.FirstOrDefault(p => p.Name == packName);

                if (toRemove != null)
                {
                    packs.Remove(toRemove);

                    var options = new JsonSerializerOptions { WriteIndented = true };
                    using FileStream stream = File.Create(_filePath);
                    await JsonSerializer.SerializeAsync(stream, packs, options);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting pack: {ex.Message}");
            }
        }
    }
}
