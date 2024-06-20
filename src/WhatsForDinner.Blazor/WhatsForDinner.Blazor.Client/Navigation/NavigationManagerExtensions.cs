using Microsoft.AspNetCore.Components;

namespace WhatsForDinner.Blazor.Client.Navigation;

internal static class NavigationManagerExtensions
{
	public static string GetSummaryHref(this NavigationManager navigation, Guid groupId) => $"/groups/{groupId}/summary";

	public static void NavigateToSummary(this NavigationManager navigation, Guid groupId) => navigation.NavigateTo(navigation.GetSummaryHref(groupId));



	public static string GetMealsHref(this NavigationManager navigation, Guid groupId) => $"/groups/{groupId}/meals";
}
