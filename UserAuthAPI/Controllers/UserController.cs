using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserAuthAPI.DAL;
using UserAuthAPI.Models;

namespace UserAuthAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserDbContext _userDbContext;

        public UserController(UserDbContext userDbContext)
        {
            _userDbContext = userDbContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            if (_userDbContext.users == null)
            {
                return NotFound();
            }
            return await _userDbContext.users.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            if (_userDbContext.users == null)
            {
                return NotFound();
            }
            var user = await _userDbContext.users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }
            return user;
        }

        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            _userDbContext.users.Add(user);
            await _userDbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<User>> PutUser(int id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }
            _userDbContext.Entry(user).State = EntityState.Modified;

            try
            {
                await _userDbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserAvailable(id))
                {
                    return BadRequest();
                }
                else
                {
                    throw;
                }
            }
            return Ok(user);
        }

        private bool UserAvailable(int id)
        {
            throw new NotImplementedException();
        }


        /* [HttpPut("{id}")]
         public async Task<ActionResult<User>> PutUser(int id, User user)
         {
             if (id != user.Id)
             {
                 return BadRequest();
             }

             _userDbContext.Entry(user).State = EntityState.Modified;

             try
             {
                 await _userDbContext.SaveChangesAsync();
             }
             catch (DbUpdateConcurrencyException)
             {
                 if (!UserExists(id))
                 {
                     return NotFound();
                 }
                 else
                 {
                     throw;
                 }
             }
             return Ok(user);
         }

         private bool UserExists(int id)
         {                                                                               
             return _userDbContext.users.Any(e => e.Id == id);
         }
 */


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            if(_userDbContext.users == null)
            {
                return NotFound();
            }
            var user = await _userDbContext.users.FindAsync(id);

            if(user == null)
            {
                return NotFound();
            }
            _userDbContext.Remove(user);
            await _userDbContext.SaveChangesAsync();

            return Ok();    
        }
    }
}
