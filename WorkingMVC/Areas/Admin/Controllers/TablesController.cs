using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AspnetCoreMvcFull.Models;
using Microsoft.AspNetCore.Authorization;
using WorkingMVC.Constants;

namespace WorkingMVC.Areas.Admin.Controllers;
[Area("Admin")]
[Authorize(Roles = $"{Roles.Admin}")]
public class TablesController : Controller
{
  public IActionResult Basic() => View();
}
