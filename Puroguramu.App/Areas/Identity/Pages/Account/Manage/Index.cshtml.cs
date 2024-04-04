// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Puroguramu.Infrastructures.dto;

namespace Puroguramu.App.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<Utilisateurs> _userManager;
        private readonly SignInManager<Utilisateurs> _signInManager;

        public IndexModel(
            UserManager<Utilisateurs> userManager,
            SignInManager<Utilisateurs> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {

            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required(ErrorMessage = "Le champ Matricule est requis")]
            [RegularExpression(@"^[a-zA-Z][0-9]{6}$", ErrorMessage = "Le matricule doit être une chaîne de 7 caractères commençant par une lettre suivie de chiffres")]
            public string Matricule { get; set; }

            [Required(ErrorMessage = "Le champ nom est requis")]
            [RegularExpression(@"^[a-zA-Z\s]*$", ErrorMessage = "Le nom doit être du texte")]
            public string Nom { get; set; }

            [Required(ErrorMessage = "Le champ prenom est requis")]
            [RegularExpression(@"^[a-zA-Z\s]*$", ErrorMessage = "Le prenom doit être du texte")]
            public string Prenom { get; set; }

            [Required(ErrorMessage = "Le champ groupe est requis")]
            [RegularExpression(@"^[0-9]$", ErrorMessage = "Le groupe doit être un chiffre")]

            public string Groupe { get; set; }

            [Display(Name = "Image de profil")]
            public IFormFile ProfilePicture { get; set; }

            public string CurrentProfilePictureBase64 { get; set; }
        }

        private async Task LoadAsync(Utilisateurs user)
        {
            var userName = await _userManager.GetUserNameAsync(user);

            Username = userName;

            Input = new InputModel
            {
                //PhoneNumber = phoneNumber

            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            // Initialisation de Input ici avant de l'utiliser
            Input = new InputModel();

            if (user.ProfilePicture != null)
            {
                // Console.WriteLine(user.ProfilePicture.Length); // Assurez-vous que cette ligne ne cause pas de problème
                Input.CurrentProfilePictureBase64 = Convert.ToBase64String(user.ProfilePicture);
            }

            await LoadAsync(user); // Cette méthode doit aussi correctement initialiser `Input`
            return Page();
        }


        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user); // Rechargez les informations en cas d'erreur
                return Page();
            }

            if (Input.ProfilePicture != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await Input.ProfilePicture.CopyToAsync(memoryStream);
                    user.ProfilePicture = memoryStream.ToArray();
                }
            }

            // Mise à jour de l'utilisateur dans la base de données
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                throw new InvalidOperationException($"Unexpected error occurred updating user with ID '{user.Id}'.");
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Votre profil a été mis à jour";
            return RedirectToPage();
        }

    }
}