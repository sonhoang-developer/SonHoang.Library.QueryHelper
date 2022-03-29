using Firebase.Auth;
using Firebase.Storage;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SonHoang.Library.Helpers
{
    public static class FileHelper
    {
        public static void DeleteFileInGitHub(this IFormFile file, string endpointUrlFile, string accessTokenGithub, string userAgent, string commiterName = "sonhoang", string commiterEmail = "sonhoang.developer@gmail.com")
        {
            var headers = new Dictionary<string, object>
            {
                { "Authorization", "Bearer " + accessTokenGithub },
                { "User-Agent", userAgent },
                { "Accept", "*/*" },
                { "Connection", "keep-alive" }
            };
            var dataBody = new Dictionary<string, object>
            {
                { "message", "APK" },
                { "sha", file.FileName },
                {
                    "committer",
                    new
                    {
                        name = commiterName,
                        email = commiterEmail
                    }
                }
            };
            string encodingData = JsonConvert.SerializeObject(dataBody);
            var resultDeleteFile = HttpMethod.Delete.SendRequestWithStringContent(endpointUrlFile, encodingData, headers);

        }

        public static string UploadFileToGithub(this IFormFile file, string endpointUrlFile, string accessTokenGithub, string userAgent, string commiterName = "sonhoang", string commiterEmail = "sonhoang.developer@gmail.com")
        {
            string result;
            var headers = new Dictionary<string, object>
            {
                { "Authorization", "Bearer " + accessTokenGithub },
                { "User-Agent", userAgent },
                { "Accept", "*/*" },
                { "Connection", "keep-alive" }
            };

            string content = file.ConvertFileToBase64String();
            var dataBody = new Dictionary<string, object>
            {
                //dataBody.Add("message", "APK");
                //dataBody.Add("sha", file.FileName);
                {
                    "committer",
                    new
                    {
                        name = commiterName,
                        email = commiterEmail
                    }
                },
                { "content", content }
            };

            string encodingData = JsonConvert.SerializeObject(dataBody);

            var (responseData, responseStatusCode) = HttpMethod.Put.SendRequestWithStringContent(endpointUrlFile, encodingData, headers);
            var jsonData = JObject.Parse(responseData);
            result = jsonData["content"]["download_url"].ToString();
            return result;
        }
        public static string ConvertFileToBase64String(this IFormFile file)
        {
            BinaryReader br = new(file.OpenReadStream());
            byte[] imageBytes = br.ReadBytes((int)file.OpenReadStream().Length);
            return Convert.ToBase64String(imageBytes);
        }

        public static async Task<string> UploadFileToFolderOnCurrentServer(this IFormFile requestFile, string folderPath)
        {
            string fileFullName = new Random().Next() + "_" + Regex.Replace(requestFile.FileName.Trim(), @"[^a-zA-Z0-9-_.]", "");
            string webRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            Directory.CreateDirectory(Path.Combine(webRootPath, folderPath)); // Automatic create folder if doesn't have yet
            using (var stream = new FileStream(Path.Combine(webRootPath, folderPath, fileFullName), FileMode.Create))
            {
                await requestFile.CopyToAsync(stream);
                stream.Close();
            }
            fileFullName = Path.Combine(folderPath, fileFullName);
            return fileFullName;
        }

        public static (bool, string) DeleteFileOnCurrentServer(this string FileToDelete)
        {
            string webRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            try
            {
                string fullPathToDelete = Path.Combine(webRootPath, FileToDelete);
                if (File.Exists(fullPathToDelete))
                {
                    File.Delete(fullPathToDelete);
                    return (true, "File deleted successfully!");
                }
                else
                {
                    return (false, "File no longer exists on the server!");
                }
            }
            catch (IOException ioExp)
            {
                return (false, ioExp.Message);
            }
        }
        public static async Task<string> UploadFileToFirebaseStorage(this IFormFile file, string endpointUrlFile, string APIKey, string Bucket)
        {
            string result = "";
            var auth = new FirebaseAuthProvider(new FirebaseConfig(APIKey));
            FirebaseAuthLink firebaseAuthLink;
            Func<Task<string>> AuthTokenAsyncFactory;
            try
            {
                firebaseAuthLink = await auth.SignInAnonymouslyAsync();
                AuthTokenAsyncFactory = () => Task.FromResult(firebaseAuthLink.FirebaseToken);
            }
            catch (Exception ex)
            {
                throw new FirebaseAuthException(endpointUrlFile, null, null, ex);
            }
            FirebaseStorageTask upload = new FirebaseStorage(Bucket, new FirebaseStorageOptions
            {
                AuthTokenAsyncFactory = AuthTokenAsyncFactory,
                ThrowOnCancel = true
            }).Child(endpointUrlFile).PutAsync(file.OpenReadStream());

            try
            {
                result = await upload;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception was thrown: {0}", ex);
                throw new FirebaseStorageException(endpointUrlFile, result, ex);
            }
            return result;
        }
        public static async Task<string> UploadFileToFirebaseStorage(this IFormFile file, string endpointUrlFile, string APIKey, string Bucket, string email, string password)
        {
            var auth = new FirebaseAuthProvider(new FirebaseConfig(APIKey));
            FirebaseAuthLink firebaseAuthLink;
            Func<Task<string>> AuthTokenAsyncFactory = null;
            try
            {
                firebaseAuthLink = await auth.SignInWithEmailAndPasswordAsync(email, password);
                AuthTokenAsyncFactory = () => Task.FromResult(firebaseAuthLink.FirebaseToken);
            }
            catch
            {
                await UploadFileToFirebaseStorage(file, endpointUrlFile, APIKey, Bucket);
            }

            FirebaseStorageTask upload = new FirebaseStorage(Bucket, new FirebaseStorageOptions
            {
                AuthTokenAsyncFactory = AuthTokenAsyncFactory,
                ThrowOnCancel = true
            }).Child(endpointUrlFile).PutAsync(file.OpenReadStream());

            try
            {
                return await upload;
            }
            catch (Exception ex)
            {

                Console.WriteLine("Exception was thrown: {0}", ex);
            }
            return null;
        }
    }
}
