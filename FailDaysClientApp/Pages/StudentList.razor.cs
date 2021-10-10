using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Blazorise;
using Blazorise.DataGrid;
using FailDaysClientApp.Controller;
using FailDaysClientApp.Models;
using FailDaysClientApp.Shared;
using Microsoft.AspNetCore.Components;

namespace FailDaysClientApp.Pages
{
    public partial class StudentList
    {
        Student selectedStudent = new(){FirstName = "", LastName = ""};
        Student[] students;
        List<Grade> gradeCategegories = new();
        int totalStudents;
        Select<double> selectRef;
        List<CustomBadge> badges = new();
        List<Row> rows = new();
        [Inject] private HttpClient httpClient { get; set; }
        [Inject] private HttpResponseController httpMessageConverter { get; set; }
    
        async Task OnReadData(DataGridReadDataEventArgs<Student> e)
        {
            HttpResponseMessage response = await httpClient.GetAsync("https://localhost:5001/api/student");
            students = await response.Content.ReadFromJsonAsync<Student[]>();
            StateHasChanged();
        }
    
        async Task OnSelectedValueChanged(int index, decimal value)
        {
            gradeCategegories[index].Number = Convert(value);
            var uri = $"https://localhost:5001/api/grade/{gradeCategegories[index].Id}";
            HttpResponseMessage responseGrades = await httpClient.PutAsJsonAsync(uri, gradeCategegories[index]);
            ShowBadgeAccordingToHttpResponse(responseGrades.StatusCode, index);
        }
    
        private async Task ShowBadgeAccordingToHttpResponse(HttpStatusCode statusCode, int index)
        {
            string message = httpMessageConverter.ConvertResponseCodeToMessage(statusCode);
            if (statusCode == HttpStatusCode.OK)
                await badges[index].Success(message);
            else
                await badges[index].Fail(message);
            StateHasChanged();
        }
    
        private void CreateLists()
        {
            rows.Clear();
            badges.Clear();
            
            foreach (var grade in gradeCategegories)
            {
                rows.Add(new Row());
                CustomBadge badge = new CustomBadge();
                badges.Add(badge);
            }
        }
        
        private decimal Convert(decimal dec)
        {
            if (dec <= 5) 
                return dec;
            return dec / 10;
        }
    
        private async Task ChangeStudentDetails(Student student)
        {
            selectedStudent = student;
            HttpResponseMessage responseGrades = await httpClient.GetAsync($"https://localhost:5001/api/grade/{student.MatNr}");
            var test = await responseGrades.Content.ReadFromJsonAsync<Grade[]>();
            gradeCategegories = test.ToList();
            CreateLists();
            StateHasChanged();
        }
    }
}