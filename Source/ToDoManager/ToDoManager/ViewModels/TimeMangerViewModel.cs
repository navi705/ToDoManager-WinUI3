using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Windows.Input;
using ToDoManager.HelpClasses;
using ToDoManager.Models;
using ToDoManager.Services.Navigation;
using ToDoManager.Services.TimeManagerService;

namespace ToDoManager.ViewModels
{
    public class TimeMangerViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly ITimeManagerService _timeManagerService;
        public TimeMangerViewModel(INavigationService navigationService, ITimeManagerService timeManagerService)
        {
            _timeManagerService = timeManagerService;
            _navigationService = navigationService;
        }

        private List<TimeNote> _allTimeTable = new();
        private List<TimeNote> _forNowDayTimeTable = new();

        public async void Intilization()
        {
            HttpResponseMessage response = await _timeManagerService.GetTimeTableAsync();
            if (response.IsSuccessStatusCode)
            {
                DatePage = DateTimeOffset.Now;
                var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase, WriteIndented = true };
                var time = JsonSerializer.Deserialize<List<TimeNote>>(response.Content.ReadAsStringAsync().Result.ToString(), options);
                if (time.Count != 0)
                {
                    _allTimeTable = ObjectExtensions.Copy(time);
                    _forNowDayTimeTable = _allTimeTable.Where(x => x.Date == DateTimeOffset.Now.Date.ToShortDateString()).ToList();
                    TimeTable = new ObservableCollection<TimeNote>(time.Where(x => x.Date == DateTimeOffset.Now.Date.ToShortDateString()).ToList());
                }
            }
        }
        private DateTimeOffset _datePage;
        public DateTimeOffset DatePage
        {
            get => _datePage;
            set { _datePage = value; OnPropertyChanged(nameof(DatePage)); changeDatePage(); }
        }
        
        private ObservableCollection<TimeNote> _timeTable = new();
        public ObservableCollection<TimeNote> TimeTable
        {
            get => _timeTable;
            set { _timeTable = value; OnPropertyChanged(nameof(TimeTable)); }
        }

        public ICommand AddTimeNote => new Command(() => addTimeNote());
        private void addTimeNote()
        {
            TimeTable.Add(new TimeNote
            {
                NameTask = "Empty",
                Date = DatePage.Date.ToShortDateString(),
                Of = DateTimeOffset.Now.TimeOfDay.ToString(@"hh\:mm"),
                To = DateTimeOffset.Now.TimeOfDay.ToString(@"hh\:mm")
            });
        }

        public ICommand SaveTimeNote => new Command(() => saveTimeNote());
        private async void saveTimeNote()
        {
            for (int i = 0; i < TimeTable.Count; ++i)
            {
                if (_forNowDayTimeTable.Count > i)
                {
                    if (TimeTable[i].NameTask != _forNowDayTimeTable[i].NameTask || TimeTable[i].Of != _forNowDayTimeTable[i].Of ||
                        TimeTable[i].To != _forNowDayTimeTable[i].To)
                    {
                        if (DateTimeOffset.Parse(TimeTable[i].Of) < DateTimeOffset.Parse(TimeTable[i].To))
                        {
                            await _timeManagerService.PutTimeTableAsync(TimeTable[i], _forNowDayTimeTable[i].NameTask);
                        }
                        else
                        {
                            //сообщение об ошибке
                        }
                    }
                }
                else
                {
                    if (DateTimeOffset.Parse(TimeTable[i].Of) < DateTimeOffset.Parse(TimeTable[i].To))
                    {
                        await _timeManagerService.PutTimeTableAsync(TimeTable[i], TimeTable[i].NameTask);
                    }
                    else
                    {
                        // сообщение об ошибке
                    }
                }
            }

        }
        public async void DeleteTimeNote(TimeNote time)
        {
            var tempTime = TimeTable;
            foreach (TimeNote times in TimeTable)
            {
                if (times == time)
                {
                    await _timeManagerService.DeleteTimeTableAsync(time);
                    tempTime.Remove(time);
                    TimeTable = tempTime;
                    break;
                }
            }
        }
        public ICommand NextDay => new Command(() => nextDay());
        private void nextDay()
        {
            DatePage = DatePage.AddDays(1);
            changeDatePage();
        }
        public ICommand PrevDay => new Command(() => prevDay());
        private void prevDay()
        {
            DatePage = DatePage.AddDays(-1);
            changeDatePage();
        }

        private void changeDatePage()
        {
            _forNowDayTimeTable = ObjectExtensions.Copy(_allTimeTable.Where(x => x.Date == DatePage.Date.ToShortDateString()).ToList());
            TimeTable = new ObservableCollection<TimeNote>(ObjectExtensions.Copy(_forNowDayTimeTable));
        }
    }
}
