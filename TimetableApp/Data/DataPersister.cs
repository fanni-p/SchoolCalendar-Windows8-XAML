using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TimetableApp.Models;
using Windows.Storage;

namespace TimetableApp.Data
{
    public class DataPersister
    {
        private static string accessToken = App.AccessToken;

        private const string BaseUrl = "http://timetableservices.apphb.com/api/";

        internal static async void RegisterUser(string username, string authenticationCode)
        {
            var userModel = new UserModel()
            {
                Username = username,
                AuthCode = authenticationCode
            };

            var registerUrl = BaseUrl + "user/register";

            await HttpRequest.Post(registerUrl, userModel);
        }

        internal static async Task<string> LoginUser(string username, string authenticationCode)
        {
            var userModel = new UserModel()
            {
                Username = username,
                AuthCode = authenticationCode
            };

            var loginUrl = BaseUrl + "auth/token";
            var loginResponse = await HttpRequest.Post<LoginResponseModel>(loginUrl, userModel);

            App.AccessToken = loginResponse.AccessToken;
            accessToken = loginResponse.AccessToken;

            var user = loginResponse.Username;

            return user;
        }

        internal static async Task<bool> LogoutUser()
        {
            bool isLogoutSuccessful;
            var logoutUrl = BaseUrl + "user/logout";

            var response = await HttpRequest.Put(logoutUrl, accessToken);

            if (response.IsSuccessStatusCode)
            {
                isLogoutSuccessful = true;
                Windows.Storage.ApplicationData.Current.RoamingSettings.Values.Remove("accessToken");
                ApplicationData.Current.SignalDataChanged();
                App.AccessToken = null;
            }
            else
            {
                isLogoutSuccessful = false;
            }

            return isLogoutSuccessful;
        }

        internal static async Task<IEnumerable<SubjectModel>> GetAllSubjects()
        {
            var subjectsUrl = BaseUrl + "subject";
            var response = await HttpRequest.Get<IEnumerable<SubjectModel>>(subjectsUrl, accessToken);

            return response;
        }

        internal static async Task<bool> AddSubject(SubjectModel subject)
        {
            bool isSubjectAdded;
            var homeworkUrl = BaseUrl + "subject";
            var response = await HttpRequest.Post(homeworkUrl, subject, accessToken);

            if (response.IsSuccessStatusCode)
            {
                isSubjectAdded = true;
            }
            else
            {
                isSubjectAdded = false;
            }

            return isSubjectAdded;
        }

        internal static async Task<bool> EditSubject(SubjectModel subject, int id)
        {
            bool isSubjectEdited;
            var homeworkUrl = BaseUrl + "subject/" + id;
            var response = await HttpRequest.Post(homeworkUrl, subject, accessToken);

            if (response.IsSuccessStatusCode)
            {
                isSubjectEdited = true;
            }
            else
            {
                isSubjectEdited = false;
            }

            return isSubjectEdited;
        }

        internal static async void DeleteSubject(int id)
        {
            var url = BaseUrl + "subject/" + id;

            await HttpRequest.Delete(url, accessToken);
        }

        internal static async Task<SubjectModel> GetSubjectById(int id)
        {
            var url = BaseUrl + "subject/" + id;

            var response = await HttpRequest.Get<SubjectModel>(url, accessToken);

            return response;
        }

        internal static async Task<IEnumerable<HomeworkModel>> GetAllHomeworks()
        {
            var subjectsUrl = BaseUrl + "homework";
            var response = await HttpRequest.Get<IEnumerable<HomeworkModel>>(subjectsUrl, accessToken);

            return response;
        }

        internal static async Task<bool> AddHomework(HomeworkModel homework)
        {
            bool isHomeworkAdded;
            var homeworkUrl = BaseUrl + "homework";
            var response = await HttpRequest.Post(homeworkUrl, homework, accessToken);

            if (response.IsSuccessStatusCode)
            {
                isHomeworkAdded = true;
            }
            else
            {
                isHomeworkAdded = false;
            }

            return isHomeworkAdded;
        }

        internal static async Task<bool> EditHomework(HomeworkModel homework, int id)
        {
            bool isHomeworkEdited;
            var homeworkUrl = BaseUrl + "homework/" + id;
            var response = await HttpRequest.Post(homeworkUrl, homework, accessToken);

            if (response.IsSuccessStatusCode)
            {
                isHomeworkEdited = true;
            }
            else
            {
                isHomeworkEdited = false;
            }

            return isHomeworkEdited;
        }

        internal static async void MarkHomeworkAsDone(int id)
        {
            var url = BaseUrl + "homework/" + id;

            await HttpRequest.Put(url, accessToken);
        }

        internal static async void DeleteHomework(int id)
        {
            var url = BaseUrl + "homework/" + id;

            await HttpRequest.Delete(url, accessToken);
        }

        internal static async Task<HomeworkModel> GetHomeworkById(int id)
        {
            var url = BaseUrl + "homework/" + id;

            var response = await HttpRequest.Get<HomeworkModel>(url, accessToken);

            return response;
        }

        internal static async Task<IEnumerable<HomeworkModel>> GetHomeworkBySubject(string subject)
        {
            var url = BaseUrl + "homework/bySubject/" + subject;

            var response = await HttpRequest.Get<IEnumerable<HomeworkModel>>(url, accessToken);

            return response;
        }

        internal static async Task<bool> AddLesson(LessonModel lesson)
        {
            bool isLessonAdded;
            var lessonUrl = BaseUrl + "lesson";
            var response = await HttpRequest.Post(lessonUrl, lesson, accessToken);

            if (response.IsSuccessStatusCode)
            {
                isLessonAdded = true;
            }
            else
            {
                isLessonAdded = false;
            }

            return isLessonAdded;
        }

        internal static async Task<IEnumerable<LessonModel>> GetLessonsPerWeek()
        {
            var subjectsUrl = BaseUrl + "lesson";
            var response = await HttpRequest.Get<IEnumerable<LessonModel>>(subjectsUrl, accessToken);

            return response;
        }

        internal static async Task<LessonModel> GetLessonById(int id)
        {
            var url = BaseUrl + "lesson/" + id;

            var response = await HttpRequest.Get<LessonModel>(url, accessToken);

            return response;
        }

        internal static async Task<IEnumerable<LessonModel>> GetLessonBySubject(string subject)
        {
            var url = BaseUrl + "lesson/bySubject/" + subject;

            var response = await HttpRequest.Get<IEnumerable<LessonModel>>(url, accessToken);

            return response;
        }

        internal static async Task<bool> DeleteLesson(int id)
        {
            bool isLessonDeleted;
            var lessonUrl = BaseUrl + "lesson/" + id;
            var response = await HttpRequest.Delete(lessonUrl, accessToken);

            if (response.IsSuccessStatusCode)
            {
                isLessonDeleted = true;
            }
            else
            {
                isLessonDeleted = false;
            }

            return isLessonDeleted;
        }

        internal static async Task<bool> EditLesson(LessonModel lesson, int id)
        {
            bool isLessonEdited;
            var homeworkUrl = BaseUrl + "lesson/" + id;
            var response = await HttpRequest.Post(homeworkUrl, lesson, accessToken);

            if (response.IsSuccessStatusCode)
            {
                isLessonEdited = true;
            }
            else
            {
                isLessonEdited = false;
            }

            return isLessonEdited;
        }
    }
}
