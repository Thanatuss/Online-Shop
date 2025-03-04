using System.Threading;
using System.Threading.Tasks;
using Application.Command.DTO.User;
using Application.Command.Utilities;
using Domain.Entity;
using MediatR;
using Persistance.DBContext;

namespace Application.Command.Services.User
{
    public class RegisterProductCommand : IRequest<OperationHandler>
    {
        public SignUpDTO SignUpDTO { get; set; }

        public RegisterProductCommand(SignUpDTO signUpDTO)
        {
            SignUpDTO = signUpDTO;
        }
    }

    public class RegisterProductHandler : IRequestHandler<RegisterProductCommand, OperationHandler>
    {
        private readonly UserValidationService _userValidationService;
        private readonly CommandDBContext _commandContext;

        public RegisterProductHandler(UserValidationService userValidationService, CommandDBContext commandDbContext)
        {
            _userValidationService = userValidationService;
            _commandContext = commandDbContext;
        }

        public async Task<OperationHandler> Handle(RegisterProductCommand request, CancellationToken cancellationToken)
        {
            var signUpDTO = request.SignUpDTO;

            //  اعتبارسنجی اینکه فیلدها خالی نباشند
            var areFieldsValid = await _userValidationService.AreFieldsNotEmpty(signUpDTO);
            if (!areFieldsValid)
            {
                return OperationHandler.Error("Your information cannot be empty!");
            }

            //  اعتبارسنجی فرمت ایمیل
            var isEmailValid = _userValidationService.IsValidEmail(signUpDTO.Email);
            if (!isEmailValid)
            {
                return OperationHandler.Error("Invalid email format!");
            }

            //  اعتبارسنجی نام کاربری
            var isUsernameValid = _userValidationService.IsValidUsername(signUpDTO.Username);
            if (!isUsernameValid)
            {
                return OperationHandler.Error("Username must be between 3 and 20 characters and contain only letters and numbers!");
            }

            //  اعتبارسنجی رمز عبور
            var isPasswordValid = _userValidationService.IsValidPassword(signUpDTO.Password);
            if (!isPasswordValid)
            {
                return OperationHandler.Error("Password must be at least 8 characters long and contain at least one uppercase letter, one number, and one special character!");
            }

            //  چک کردن تکراری بودن ایمیل یا نام کاربری
            var isEmailOrUsernameDuplicate = await _userValidationService.IsDuplicateExistVRegister(signUpDTO);
            if (isEmailOrUsernameDuplicate)
            {
                return OperationHandler.Error("Your Email or Username already exists!");
            }

            // افزودن کاربر جدید به پایگاه داده
            var user = new Domain.Entity.User()
            {
                Email = signUpDTO.Email,
                Fullname = signUpDTO.Fullname,
                IsDeleted = false,
                Password = signUpDTO.Password, // توجه: باید رمز عبور هش شده باشد
                Role = UserRole.User,
                Username = signUpDTO.Username
            };

            await _commandContext.Users.AddAsync(user, cancellationToken);
            await _commandContext.SaveChangesAsync(cancellationToken);

            return OperationHandler.Success("Your account has been created successfully!");
        }
    }
}
