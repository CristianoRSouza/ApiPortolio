using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ApiEntregasMentoria.Data.Dto;
using ApiEntregasMentoria.Services;
using System.Security.Claims;

namespace ApiEntregasMentoria.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TransactionController : ControllerBase
    {
        private readonly TransactionService _transactionService;

        public TransactionController(TransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpGet]
        public async Task<ActionResult<List<TransactionDto>>> GetTransactions()
        {
            try
            {
                var userId = GetUserId();
                var transactions = await _transactionService.GetTransactionsByUserAsync(userId);
                return Ok(transactions);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPost("deposit")]
        public async Task<ActionResult<PixPaymentDto>> CreateDeposit([FromBody] DepositRequestDto request)
        {
            if (request.Amount < 10 || request.Amount > 10000)
            {
                return BadRequest(new { message = "Amount must be between R$ 10.00 and R$ 10,000.00" });
            }

            try
            {
                var userId = GetUserId();
                var pixPayment = await _transactionService.CreateDepositAsync(userId, request);
                return Ok(pixPayment);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPost("confirm-deposit/{transactionId}")]
        public async Task<IActionResult> ConfirmDeposit(int transactionId)
        {
            try
            {
                var userId = GetUserId();
                var newBalance = await _transactionService.ConfirmDepositAsync(userId, transactionId);
                return Ok(new { message = "Deposit confirmed successfully", newBalance });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPost("withdraw")]
        public async Task<IActionResult> CreateWithdraw([FromBody] WithdrawRequestDto request)
        {
            if (request.Amount < 20)
            {
                return BadRequest(new { message = "Minimum withdrawal amount: R$ 20.00" });
            }

            try
            {
                var userId = GetUserId();
                await _transactionService.CreateWithdrawAsync(userId, request);
                return Ok(new { message = "Withdrawal requested successfully. Processing within 24h." });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet("balance")]
        public async Task<ActionResult<decimal>> GetBalance()
        {
            try
            {
                var userId = GetUserId();
                var balance = await _transactionService.GetUserBalanceAsync(userId);
                return Ok(new { balance });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        private int GetUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.Parse(userIdClaim!);
        }
    }
}
