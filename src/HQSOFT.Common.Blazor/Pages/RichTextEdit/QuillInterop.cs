using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HQSOFT.Common.Blazor.Pages.RichTextEdit
{
	public static class QuillInterop
	{
		private const string strCreateQuill = "QuillFunctions.createQuill";
		private const string strGetText = "QuillFunctions.getQuillText";
		private const string strGetHTML = "QuillFunctions.getQuillHTML";
		private const string strGetContent = "QuillFunctions.getQuillContent";
		private const string strLoadQuillContent = "QuillFunctions.loadQuillContent";
		private const string strEnableQuillEditor = "QuillFunctions.enableQuillEditor";
		private const string strGetMentionedUsers = "QuillFunctions.getMentionedUsers";
		private const string strClearContent = "QuillFunctions.clearContent";

		internal static ValueTask<object> CreateQuill(
			IJSRuntime jsRuntime,
			ElementReference quillElement,
			ElementReference toolbar,
			object mentionList,
			bool readOnly,
			string placeholder,
			string theme,
			string debugLevel)
		{
			return jsRuntime.InvokeAsync<object>(
				strCreateQuill,
				quillElement, toolbar, mentionList, readOnly,
				placeholder, theme, debugLevel);
		}

		internal static ValueTask<string> GetText(
			IJSRuntime jsRuntime,
			ElementReference quillElement)
		{
			return jsRuntime.InvokeAsync<string>(
				strGetText,
				quillElement);
		}

		internal static ValueTask<string> GetHTML(
			IJSRuntime jsRuntime,
			ElementReference quillElement)
		{
			return jsRuntime.InvokeAsync<string>(
				strGetHTML,
				quillElement);
		}

		internal static ValueTask<string> GetContent(
			IJSRuntime jsRuntime,
			ElementReference quillElement)
		{
			return jsRuntime.InvokeAsync<string>(
				strGetContent,
				quillElement);
		}

		internal static ValueTask<object> LoadQuillContent(
			IJSRuntime jsRuntime,
			ElementReference quillElement,
			string Content)
		{
			return jsRuntime.InvokeAsync<object>(
				strLoadQuillContent,
				quillElement, Content);
		}

		internal static ValueTask<object> EnableQuillEditor(
			IJSRuntime jsRuntime,
			ElementReference quillElement,
			bool mode)
		{
			return jsRuntime.InvokeAsync<object>(
				strEnableQuillEditor,
				quillElement, mode);
		}

		internal static ValueTask<IEnumerable<string>> GetMentionedUsers(
			IJSRuntime jsRuntime,
			ElementReference quillElement)
		{
			return jsRuntime.InvokeAsync<IEnumerable<string>>(
				strGetMentionedUsers,
				quillElement);
		}

		internal static ValueTask ClearAsync(
			IJSRuntime jsRuntime,
			ElementReference quillElement)
		{
			return jsRuntime.InvokeVoidAsync(
				strClearContent,
				quillElement);
		}
	}
}
