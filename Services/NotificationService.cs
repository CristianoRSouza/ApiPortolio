using ApiEntregasMentoria.Data.ContextEntity;
using ApiEntregasMentoria.Data.Entities;
using ApiEntregasMentoria.Data.Dto;
using Microsoft.EntityFrameworkCore;

namespace ApiEntregasMentoria.Services
{
    public class NotificationService
    {
        private readonly MyContext _context;

        public NotificationService(MyContext context)
        {
            _context = context;
        }

        public async Task<List<NotificationDto>> GetNotificationsByUserAsync(int userId)
        {
            return await _context.Set<Notification>()
                .Where(n => n.UserId == userId)
                .OrderByDescending(n => n.CreatedAt)
                .Select(n => new NotificationDto
                {
                    Id = n.Id,
                    UserId = n.UserId,
                    Type = n.Type,
                    Title = n.Title,
                    Message = n.Message,
                    IsRead = n.IsRead,
                    CreatedAt = n.CreatedAt
                })
                .ToListAsync();
        }

        public async Task MarkAsReadAsync(int notificationId, int userId)
        {
            var notification = await _context.Set<Notification>()
                .FirstOrDefaultAsync(n => n.Id == notificationId && n.UserId == userId);
            
            if (notification != null)
            {
                notification.IsRead = true;
                await _context.SaveChangesAsync();
            }
        }

        public async Task MarkAllAsReadAsync(int userId)
        {
            await _context.Set<Notification>()
                .Where(n => n.UserId == userId && !n.IsRead)
                .ExecuteUpdateAsync(n => n.SetProperty(x => x.IsRead, true));
        }

        public async Task<int> GetUnreadCountAsync(int userId)
        {
            return await _context.Set<Notification>()
                .CountAsync(n => n.UserId == userId && !n.IsRead);
        }

        public async Task SendBetResultNotification(int userId, string ticketId, bool isWinner, decimal amount)
        {
            var notification = new Notification
            {
                UserId = userId,
                Type = "bet_result",
                Title = isWinner ? "🎉 Aposta Ganha!" : "😔 Aposta Perdida",
                Message = isWinner 
                    ? $"Parabéns! Você ganhou R$ {amount:F2} no bilhete {ticketId}"
                    : $"Que pena! O bilhete {ticketId} não foi premiado desta vez",
                IsRead = false,
                CreatedAt = DateTime.UtcNow
            };

            _context.Set<Notification>().Add(notification);
            await _context.SaveChangesAsync();

            await SendPushNotification(userId, notification.Title, notification.Message);
        }

        public async Task SendBalanceUpdateNotification(int userId, decimal amount, string type)
        {
            var notification = new Notification
            {
                UserId = userId,
                Type = "balance_update",
                Title = type == "deposit" ? "💰 Depósito Confirmado" : "💸 Saque Processado",
                Message = $"Sua conta foi {(type == "deposit" ? "creditada" : "debitada")} em R$ {Math.Abs(amount):F2}",
                IsRead = false,
                CreatedAt = DateTime.UtcNow
            };

            _context.Set<Notification>().Add(notification);
            await _context.SaveChangesAsync();

            await SendPushNotification(userId, notification.Title, notification.Message);
        }

        private async Task SendPushNotification(int userId, string title, string message)
        {
            Console.WriteLine($"Push to User {userId}: {title} - {message}");
            await Task.CompletedTask;
        }
    }
}