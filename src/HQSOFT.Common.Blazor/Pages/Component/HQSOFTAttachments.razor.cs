using Blazorise;
using Blazorise.Snackbar;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Components.Messages;
using Volo.Abp.Content;
using Volo.Abp.Data;
using Volo.FileManagement.Directories;
using Volo.FileManagement.Files;

namespace HQSOFT.Common.Blazor.Pages.Component
{
	public partial class HQSOFTAttachments
	{
		[Parameter]
		public Guid DocId { get; set; }
		[Parameter]
		public string DocUrl { get; set; }
		[Parameter]
		public string DirectoryName { get; set; }

		SnackbarStack snackbarStack;
		private Modal EditImageModal { get; set; } = new();
		private Modal EditUploadModal { get; set; } = new();
		private Modal EditCameraModal { get; set; } = new();
		private Guid idImage { get; set; } = Guid.Empty;
		private Guid directoryId { get; set; }

		private CreateDirectoryInput CreateDirectory { get; set; }

		private bool isFrontCamera = true; // Trạng thái của camera (true: trước, false: sau)
		private string filePath { get; set; } = "";
		private string fileType { get; set; } = "";
		private string nameFile { get; set; } = "";
		private string fileExtension { get; set; } = "";
		private string base64Image { get; set; } = "";
		private string base64Txt { get; set; } = "";
		private long maxSize { get; set; } = (1024 * 1024 * 200); // 200MB

		private List<Guid> uploadedFileIds = new List<Guid>();
		private List<string> base64Images = new List<string>();
		private List<string> base64Txts = new List<string>();
		private List<string> fileNames = new List<string>();
		private List<string> directoryNames = new List<string>();
		private List<string> cameraList = new List<string>();

		private readonly IUiMessageService _uiMessageService;
		private readonly IMessageService _messageService;

		public HQSOFTAttachments(IUiMessageService uiMessageService, IMessageService messageService)
		{
			_uiMessageService = uiMessageService;
			_messageService = messageService;
		}

		protected override async Task OnInitializedAsync()
		{
			await GetDirectoryAsync();
			await GetFileContent();
			await base.OnInitializedAsync(); // Call the base method
		}

		private async Task GetDirectoryAsync()
		{
			// Get the directory
			var searchDirectoryList = await DirectoryDescriptorAppService.GetListAsync(null); // Get all directories
			var directory = searchDirectoryList.Items.FirstOrDefault(d => d.Name == DirectoryName); // Get the directory with DirectoryName

			// Check if the Inventory directory exists
			if (directory != null) // If it exists, get the DirectoryId of that folder
			{
				directoryId = directory.Id;
			}
			else // If it doesn't exist, create a new folder named DirectoryName
			{
				Console.WriteLine("Have not Directory");
				CreateDirectory = new CreateDirectoryInput
				{
					Name = DirectoryName,
					ParentId = null
				};
				var createdDirectory = await DirectoryDescriptorAppService.CreateAsync(CreateDirectory);
				directoryId = createdDirectory.Id;
			}
			await InvokeAsync(StateHasChanged);
		}

		private async Task OnFileSelection(InputFileChangeEventArgs e)
		{
			if (DocId != Guid.Empty)
			{
				if (e.File.Size > maxSize) // 200MB
				{
					await _uiMessageService.Warn(L["Notification:MaximumSize"] +$"{maxSize}");
				}

				string fileExtension = Path.GetExtension(e.File.Name); // Get the file extension
				string randomFileName = GenerateRandomFileName();
				if (IsImageFile(fileExtension))
				{
					filePath = randomFileName + fileExtension;
				}
				else
				{
					filePath = e.File.Name;
				}
				Console.WriteLine("filePath " + filePath);
				Console.WriteLine("IsImageFile(fileExtension) " + IsImageFile(fileExtension));
				using (var stream = e.File.OpenReadStream(maxAllowedSize: maxSize))
				{
					var editingFile = new CreateFileInputWithStream
					{
						Name = filePath,
						File = new RemoteStreamContent(
									stream: stream,
									fileName: filePath,
									contentType: e.File.ContentType,
									disposeStream: true
								)
					};

					editingFile.SetProperty("Url", DocUrl);
					editingFile.SetProperty("DocId", DocId);

					var uploadedFile = await FileDescriptorAppService.CreateAsync(directoryId, editingFile);
					await GetFileContent();
					await Notify.Success(L["Notification:Save"]);
					await CloseEditUploadModalAsync();
					await InvokeAsync(StateHasChanged);
				}
			}
			else
			{
				await _uiMessageService.Warn(L["Message:SelectAnImage"]);
			}
		}

		private async Task DeleteFile(Guid fileId)
		{
			var confirmed = await _uiMessageService.Confirm(L["DeleteConfirmationMessage"]);
			if (confirmed)
			{
				await FileDescriptorAppService.DeleteAsync(fileId);
				await GetFileContent();
			}
		}

		#region Get File & Image 
		private async Task GetFileContent()
		{
			var searchFileList = await FileDescriptorAppService.GetListAsync(directoryId);
			var files = searchFileList.Items;

			directoryNames.Clear();
			uploadedFileIds.Clear();
			fileNames.Clear();

			foreach (var file in files)
			{
				var extraProperties = file.ExtraProperties;
				var docId = extraProperties["DocId"];
				var url = extraProperties["Url"];

				if (Guid.TryParse((string)docId, out Guid docGuid) && docGuid == DocId && url.ToString() == DocUrl)
				{
					directoryNames.Add(DirectoryName);
					uploadedFileIds.Add(file.Id);
					fileNames.Add(file.Name);
				}
			}
		} 
		private async Task GetImages()
		{
			var searchImageList = await FileDescriptorAppService.GetListAsync(directoryId);
			var images = searchImageList.Items.ToList();
			foreach (var image in images.Where(x => x.Name == nameFile))
			{
				idImage = image.Id;
			}
			byte[] fileContent = await FileDescriptorAppService.GetContentAsync(idImage);
			base64Image = Convert.ToBase64String(fileContent);
			base64Images.Add(base64Image);
			await InvokeAsync(StateHasChanged);
		}
		private async Task GetContents()
		{
			var searchImageList = await FileDescriptorAppService.GetListAsync(directoryId);
			var images = searchImageList.Items.ToList();
			foreach (var image in images.Where(x => x.Name == nameFile))
			{
				idImage = image.Id;
			}
			byte[] fileContent = await FileDescriptorAppService.GetContentAsync(idImage);
			string decodedContent = Encoding.UTF8.GetString(fileContent);
			base64Txt = Convert.ToBase64String(fileContent);
			base64Txts.Add(base64Image);
			await InvokeAsync(StateHasChanged);
		}
		#endregion

		#region Open & Close Modal
		private async Task OpenShowImageModalAsync(string fileName)
		{
			nameFile = fileName;
			base64Image = ""; // Reset base64Image to empty string
			base64Txt = ""; // Reset base64Image to empty string
			fileExtension = Path.GetExtension(nameFile); // Get the file extension
			await GetContents();
			await GetImages();
			await EditImageModal.Show();
		}
		private async Task OpenUploadModalAsync()
		{
			await EditUploadModal.Show();
		}
		private async Task OpenCameraModalAsync()
		{
			await JSRuntime.InvokeVoidAsync("ready");
			await EditCameraModal.Show();
		}

		private async Task CloseEditImageModalAsync()
		{
			await EditImageModal.Hide();
		}
		private async Task CloseEditUploadModalAsync()
		{
			await EditUploadModal.Hide();
		}
		private async Task CloseEditCameraModalAsync()
		{
			await JSRuntime.InvokeVoidAsync("stopCamera");
			await EditCameraModal.Hide();
		}
		#endregion

		#region Camera
		private async Task Capture()
		{
			var maxFilesToShow = 5;
			var numImagesToSave = maxFilesToShow - cameraList.Count;

			if (numImagesToSave > 0)
			{
				var capturedImage = await JSRuntime.InvokeAsync<string>("take_snapshot");
				cameraList.Add(capturedImage);
				await InvokeAsync(StateHasChanged);
			}
			else
			{
				await _uiMessageService.Error(L["Notification:MaximumFile"]);
			}
		} 
		private async Task SwitchCamera()
		{
			await JSRuntime.InvokeVoidAsync("switchCamera");
			isFrontCamera = !isFrontCamera;
			await InvokeAsync(StateHasChanged);
		}
		private string GetCameraState()
		{
			return isFrontCamera ? "Front Camera" : "Back Camera";
		}
		public async Task ClearImage()
		{
			cameraList.Clear();
			await InvokeAsync(StateHasChanged);
		}
		private bool IsBase64String(string value)
		{
			try
			{
				Convert.FromBase64String(value);
				return true;
			}
			catch (FormatException)
			{
				return false;
			}
		}
		private async Task SaveToDatabase()
		{
			var maxFilesToShow = 5;
			var numImagesToSave = maxFilesToShow - cameraList.Count;

			if (cameraList.Count <= numImagesToSave)
			{
				foreach (var image in cameraList)
				{
					try
					{
						// Extract the Base64 string from the image data
						string base64String = image.Substring(image.IndexOf(',') + 1);

						// Check if the captured image string is a valid Base64 string
						if (IsBase64String(base64String))
						{
							// Convert the captured image to a byte array
							byte[] imageBytes = Convert.FromBase64String(base64String);
							string randomFileName = GenerateRandomFileName();
							filePath = randomFileName + ".jpeg";
							using (var stream = new MemoryStream(imageBytes))
							{
								var editingFile = new CreateFileInputWithStream
								{
									Name = filePath,
									File = new RemoteStreamContent(
										stream: stream,
										fileName: filePath,
										contentType: "image/jpeg",
										disposeStream: true
										)
								};
								editingFile.SetProperty("Url", DocUrl);
								editingFile.SetProperty("DocId", DocId);
								var uploadedFile = await FileDescriptorAppService.CreateAsync(directoryId, editingFile);
								await GetFileContent();
								await Notify.Success(L["Notification:Save"]);
								await CloseEditCameraModalAsync();
								await CloseEditUploadModalAsync();
								await InvokeAsync(StateHasChanged);
							}
						}
						else
						{
							Console.WriteLine("Invalid Base64 string");
						}
					}
					catch (FormatException ex)
					{
						Console.WriteLine($"Error decoding Base64 string: {ex.Message}");
					}
				}
				cameraList.Clear();
				await InvokeAsync(StateHasChanged);
			}
			else
			{
				await _uiMessageService.Error(L["Notification:MaximumFile"]);
			}
		}

		#endregion

		#region Download File

		private DownloadTokenResultDto getToken = new DownloadTokenResultDto();
		private async Task DownloadFile(string fileName)
		{
			var searchImageList = await FileDescriptorAppService.GetListAsync(directoryId);
			var images = searchImageList.Items.ToList();
			foreach (var image in images.Where(x => x.Name == nameFile))
			{
				idImage = image.Id;
			}
			getToken = await FileDescriptorAppService.GetDownloadTokenAsync(idImage);
			await FileDescriptorAppService.DownloadAsync(idImage, getToken.Token);
			var remoteService = await RemoteServiceConfigurationProvider.GetConfigurationOrDefaultOrNullAsync("Attachments") ??
			await RemoteServiceConfigurationProvider.GetConfigurationOrDefaultOrNullAsync("Default");
			NavigationManager.NavigateTo($"{remoteService?.BaseUrl.EnsureEndsWith('/') ?? string.Empty}api/file-management/file-descriptor/download/{idImage}?token={getToken.Token}", forceLoad: true);
		}
		#endregion

		#region Check is image file
		private bool IsImageFile(string fileExtension)
		{
			string[] supportedImageExtensions = { ".jpg", ".jpeg", ".png", ".gif" }; // Các phần mở rộng tệp tin hình ảnh được hỗ trợ

			return supportedImageExtensions.Contains(fileExtension);
		}

		private bool IsFileExtension(string fileExtension)
		{
			string[] supportedImageExtensions = { ".txt", ".jpg", ".jpeg", ".png", ".gif", ".pdf", ".docx", ".xlsx" }; // Các phần mở rộng tệp tin hình ảnh được hỗ trợ

			return supportedImageExtensions.Contains(fileExtension);
		}
		#endregion

		#region Random file name
		private string GenerateRandomFileName()
		{
			const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
			var random = new Random();
			var fileName = new string(Enumerable.Repeat(chars, 7)
				.Select(s => s[random.Next(s.Length)]).ToArray());
			return fileName;
		}
		#endregion
		 
		#region Get Multiple Files
		private async Task OnFilesSelection(InputFileChangeEventArgs e)
		{
			if (DocId != Guid.Empty)
			{
				var files = e.GetMultipleFiles(); // Get the files selected by the users
				foreach (var file in files)
				{
					if (file.Size > maxSize) // 1GB
					{
						await _uiMessageService.Warn(L["The selected file exceeds the maximum allowed size of 200MB."]);
						continue; // Skip to the next file if it exceeds the size limit
					}

					string fileExtension = Path.GetExtension(file.Name); // Get the file extension
					string randomFileName = GenerateRandomFileName();
					if (IsImageFile(fileExtension))
					{
						filePath = randomFileName + fileExtension;
					}
					else
					{
						filePath = file.Name;
					}
					Console.WriteLine("filePath " + filePath);
					Console.WriteLine("IsImageFile(fileExtension) " + IsImageFile(fileExtension));
					using (var stream = file.OpenReadStream(maxAllowedSize: maxSize))
					{
						var editingFile = new CreateFileInputWithStream
						{
							Name = filePath,
							File = new RemoteStreamContent(
										stream: stream,
										fileName: filePath,
										contentType: file.ContentType,
										disposeStream: true
									)
						};

						editingFile.SetProperty("Url", DocUrl);
						editingFile.SetProperty("DocId", DocId);

						var uploadedFile = await FileDescriptorAppService.CreateAsync(directoryId, editingFile);
						await GetFileContent();
						await CloseEditUploadModalAsync();
						await InvokeAsync(StateHasChanged);
					}
				}
			}
			else
			{
				await _uiMessageService.Warn(L["Message:SelectAnImage"]);
			}
		}
		#endregion
	}
}
