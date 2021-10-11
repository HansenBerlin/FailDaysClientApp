using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
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
        List<Grade> currentStudentGrades = new();
        int totalStudents;
        Select<double> selectRef;
        //List<CustomBadge> badges = new();
        //List<Row> rows = new();
        [Inject] private HttpClient httpClient { get; set; }
        [Inject] private HttpResponseController httpMessageConverter { get; set; }
        [Inject] private GradeCalculationController gradeCalculator { get; set; }
    
        private void Callback(object args, int index)
        {
            decimal value = ConvertCultureSpecific((string)args);
            currentStudentGrades[index].Number = ConvertBaseTen(value);
            var uri = $"https://localhost:5001/api/grade/{currentStudentGrades[index].Id}";
            HttpResponseMessage responseGrades = httpClient.PutAsJsonAsync(uri, currentStudentGrades[index]).Result;
            ShowBadgeAccordingToHttpResponse(responseGrades.StatusCode, index);
            StateHasChanged();
        }

        private decimal ConvertCultureSpecific(string input)
        {
            NumberFormatInfo numberFormatWithComma = new NumberFormatInfo();
            numberFormatWithComma.NumberDecimalSeparator = ",";
            return decimal.Parse(input, numberFormatWithComma);
        }

        async Task Start()
        {
            HttpResponseMessage response = await httpClient.GetAsync("https://localhost:5001/api/student");
            students = await response.Content.ReadFromJsonAsync<Student[]>();
            StateHasChanged();
        }
        
        protected override void OnAfterRender(bool firstRender)
        {
            if(firstRender)
            {
                Start();
            }
        }
    
        async Task OnSelectedValueChanged(int index, decimal value)
        {
            currentStudentGrades[index].Number = ConvertBaseTen(value);
            var uri = $"https://localhost:5001/api/grade/{currentStudentGrades[index].Id}";
            HttpResponseMessage responseGrades = await httpClient.PutAsJsonAsync(uri, currentStudentGrades[index]);
            ShowBadgeAccordingToHttpResponse(responseGrades.StatusCode, index);
            StateHasChanged();
        }
    
        private async Task ShowBadgeAccordingToHttpResponse(HttpStatusCode statusCode, int index)
        {
            string message = httpMessageConverter.ConvertResponseCodeToMessage(statusCode);
            //var test = new ToastMessage(message);
            
            if (statusCode == HttpStatusCode.OK) 
                await dropdowns[index].AttachedBage.Success(message);
            else
                await dropdowns[index].AttachedBage.Fail(message);
            //StateHasChanged();
        }

        private List<CustomDropdown> dropdowns = new();
        private void CreateLists()
        {
            //rows.Clear();
            //badges.Clear();
            dropdowns.Clear();
            
            foreach (var grade in currentStudentGrades)
            {
                var dropdown = new CustomDropdown();
                dropdowns.Add(dropdown);
                //rows.Add(new Row());
                //CustomBadge badge = new CustomBadge();
                //badges.Add(badge);
            }
        }
        
        private decimal ConvertBaseTen(decimal dec)
        {
            if (dec <= 5) 
                return dec;
            return dec / 10;
        }
    
        private async Task ChangeStudentDetails(Student student)
        {
            selectedStudent = student;
            HttpResponseMessage responseGrades = await httpClient.GetAsync($"https://localhost:5001/api/grade/{student.MatNr}");
            var grades = await responseGrades.Content.ReadFromJsonAsync<Grade[]>();
            currentStudentGrades = grades.ToList();
            selectedStudent.AverageGrade = gradeCalculator.CalculateAverageGrade(currentStudentGrades);
            CreateLists();
            //StateHasChanged();
        }
    }
}