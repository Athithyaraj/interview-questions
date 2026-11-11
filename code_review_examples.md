# Corpay Interview - Additional Code Examples & Best Practices

## 1. ASYNC/AWAIT PATTERNS (Important for Backend Services)

### ✅ GOOD: Proper Async Pattern
```csharp
public async Task<EmailValidationResult> ValidateEmailAsync(string email)
{
    if (string.IsNullOrWhiteSpace(email))
        return EmailValidationResult.Invalid("Email is empty");
    
    try
    {
        // Simulate async operation (e.g., calling external service)
        var result = await CheckEmailWithServiceAsync(email.Trim());
        return result;
    }
    catch (Exception ex)
    {
        return EmailValidationResult.Invalid($"Validation failed: {ex.Message}");
    }
}

private async Task<EmailValidationResult> CheckEmailWithServiceAsync(string email)
{
    // Simulating API call
    await Task.Delay(100);
    return EmailValidationResult.Valid(email.ToLowerInvariant());
}
```

### ❌ BAD: Sync over Async (Blocks Thread)
```csharp
// DON'T DO THIS - causes deadlocks!
public EmailValidationResult ValidateEmail(string email)
{
    var result = ValidateEmailAsync(email).Result;  // BLOCKS!
    return result;
}
```

### ❌ BAD: Fire and Forget (Loses Exceptions)
```csharp
// DON'T DO THIS - exceptions are swallowed
ValidateEmailAsync(email);  // Fire and forget - exceptions lost!
```

---

## 2. RESTFUL API DESIGN

### ✅ GOOD: Proper API Endpoint
```csharp
[ApiController]
[Route("api/[controller]")]
public class EmailValidationController : ControllerBase
{
    private readonly IEmailValidationService _emailService;
    
    public EmailValidationController(IEmailValidationService emailService)
    {
        _emailService = emailService;
    }
    
    /// <summary>
    /// Validates an email address
    /// </summary>
    /// <param name="email">Email to validate</param>
    /// <returns>Validation result with normalized email</returns>
    [HttpPost("validate")]
    [ProducesResponseType(typeof(EmailValidationResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<EmailValidationResponse>> ValidateEmail(
        [FromBody] EmailValidationRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(new ErrorResponse { Message = "Invalid request" });
        
        var result = await _emailService.ValidateAsync(request.Email);
        
        if (!result.IsValid)
            return BadRequest(new ErrorResponse { Message = result.ErrorMessage });
        
        return Ok(new EmailValidationResponse
        {
            IsValid = result.IsValid,
            NormalizedEmail = result.NormalizedEmail
        });
    }
}

// Request/Response DTOs
public class EmailValidationRequest
{
    [Required]
    [StringLength(255)]
    public string Email { get; set; }
}

public class EmailValidationResponse
{
    public bool IsValid { get; set; }
    public string NormalizedEmail { get; set; }
}

public class ErrorResponse
{
    public string Message { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}
```

---

## 3. DEPENDENCY INJECTION & SERVICE LAYER

### ✅ GOOD: Dependency Injection
```csharp
// Service Interface
public interface IEmailValidationService
{
    Task<EmailValidationResult> ValidateAsync(string email);
}

// Service Implementation
public class EmailValidationService : IEmailValidationService
{
    private readonly ILogger<EmailValidationService> _logger;
    private readonly IEmailRepository _repository;
    
    public EmailValidationService(
        ILogger<EmailValidationService> logger,
        IEmailRepository repository)
    {
        _logger = logger;
        _repository = repository;
    }
    
    public async Task<EmailValidationResult> ValidateAsync(string email)
    {
        _logger.LogInformation("Validating email: {email}", email);
        
        if (string.IsNullOrWhiteSpace(email))
            return EmailValidationResult.Invalid("Email is required");
        
        var trimmed = email.Trim().ToLowerInvariant();
        
        // Check if already validated
        var cached = await _repository.GetValidatedEmailAsync(trimmed);
        if (cached != null)
            return EmailValidationResult.Valid(trimmed);
        
        // Validate
        if (!IsValidFormat(trimmed))
            return EmailValidationResult.Invalid("Invalid email format");
        
        // Cache result
        await _repository.SaveValidatedEmailAsync(trimmed);
        
        return EmailValidationResult.Valid(trimmed);
    }
    
    private bool IsValidFormat(string email)
    {
        try
        {
            new System.Net.Mail.MailAddress(email);
            return true;
        }
        catch
        {
            return false;
        }
    }
}

// Startup Configuration
public void ConfigureServices(IServiceCollection services)
{
    services.AddScoped<IEmailValidationService, EmailValidationService>();
    services.AddScoped<IEmailRepository, EmailRepository>();
    services.AddControllers();
}
```

---

## 4. ERROR HANDLING & RESULT PATTERN

### ✅ GOOD: Result Pattern with Errors
```csharp
public class Result<T>
{
    public bool Success { get; set; }
    public T Data { get; set; }
    public string ErrorMessage { get; set; }
    
    public static Result<T> Ok(T data) => new Result<T> 
    { 
        Success = true, 
        Data = data 
    };
    
    public static Result<T> Fail(string errorMessage) => new Result<T> 
    { 
        Success = false, 
        ErrorMessage = errorMessage 
    };
}

// Usage
public async Task<Result<EmailValidationResponse>> ValidateEmail(string email)
{
    if (string.IsNullOrWhiteSpace(email))
        return Result<EmailValidationResponse>.Fail("Email is required");
    
    var trimmed = email.Trim();
    
    try
    {
        var mailAddress = new System.Net.Mail.MailAddress(trimmed);
        var response = new EmailValidationResponse
        {
            IsValid = true,
            NormalizedEmail = trimmed.ToLowerInvariant()
        };
        return Result<EmailValidationResponse>.Ok(response);
    }
    catch (Exception ex)
    {
        return Result<EmailValidationResponse>.Fail($"Email validation failed: {ex.Message}");
    }
}

// Test it
[Fact]
public async Task ValidateEmail_WithValidEmail_ReturnsSuccess()
{
    var result = await ValidateEmail("user@example.com");
    Assert.True(result.Success);
    Assert.NotNull(result.Data);
}

[Fact]
public async Task ValidateEmail_WithInvalidEmail_ReturnsFail()
{
    var result = await ValidateEmail("invalid");
    Assert.False(result.Success);
    Assert.NotNull(result.ErrorMessage);
}
```

---

## 5. LOGGING & OBSERVABILITY

### ✅ GOOD: Structured Logging
```csharp
public class EmailService
{
    private readonly ILogger<EmailService> _logger;
    
    public async Task<Result<Email>> ProcessEmailAsync(string email)
    {
        _logger.LogInformation(
            "Processing email. Email={email}, Timestamp={timestamp}",
            email,
            DateTime.UtcNow);
        
        try
        {
            var validated = await ValidateAsync(email);
            
            _logger.LogInformation(
                "Email validated successfully. Email={email}, Normalized={normalized}",
                email,
                validated.NormalizedEmail);
            
            return Result<Email>.Ok(validated);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Email validation failed. Email={email}, Error={error}",
                email,
                ex.Message);
            
            return Result<Email>.Fail("Email validation failed");
        }
    }
}
```

### ✅ GOOD: Exception Handling
```csharp
public class GlobalExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;
    
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Argument validation failed");
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsJsonAsync(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception occurred");
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsJsonAsync(new { message = "Internal server error" });
        }
    }
}
```

---

## 6. LINQ BEST PRACTICES

### ✅ GOOD: Efficient LINQ
```csharp
// Good: Single pass, efficient
public List<Email> FilterValidEmails(List<string> emails)
{
    return emails
        .Where(e => !string.IsNullOrWhiteSpace(e))
        .Select(e => e.Trim())
        .Where(e => IsValidEmail(e))
        .Select(e => new Email { Address = e.ToLowerInvariant() })
        .ToList();
}

// Good: Using FirstOrDefault instead of Where then First
public Email FindEmail(List<Email> emails, string address)
{
    return emails.FirstOrDefault(e => e.Address == address);
}

// Good: Count when you need count, not Count()
public bool HasAnyValidEmails(IEnumerable<Email> emails)
{
    return emails.Any(e => e.IsValid);  // Short-circuits after first match
}
```

### ❌ BAD: Inefficient LINQ
```csharp
// Bad: Multiple enumerations
foreach (var email in emails.Where(e => e.IsValid).ToList())
{
    ProcessEmail(email);
}
foreach (var email in emails.Where(e => e.IsValid).ToList())  // Evaluated again!
{
    SendEmail(email);
}

// Bad: Using Count() to check existence
if (emails.Where(e => e.IsValid).Count() > 0)  // Counts ALL!
{
    // Process
}

// Good: Use Any() instead
if (emails.Any(e => e.IsValid))  // Stops after first match
{
    // Process
}
```

---

## 7. LEETCODE PROBLEMS - More Examples

### Problem 1: Reverse Array
```csharp
public class Solution {
    // Time: O(n), Space: O(1) in-place
    public void ReverseArray(int[] nums)
    {
        int left = 0;
        int right = nums.Length - 1;
        
        while (left < right)
        {
            int temp = nums[left];
            nums[left] = nums[right];
            nums[right] = temp;
            
            left++;
            right--;
        }
    }
}

[TestClass]
public class ReverseArrayTests
{
    [TestMethod]
    public void ReverseArray_WithValidInput_ReversesInPlace()
    {
        var solution = new Solution();
        int[] nums = { 1, 2, 3, 4, 5 };
        
        solution.ReverseArray(nums);
        
        int[] expected = { 5, 4, 3, 2, 1 };
        CollectionAssert.AreEqual(expected, nums);
    }
    
    [TestMethod]
    public void ReverseArray_WithSingleElement_DoesNothing()
    {
        var solution = new Solution();
        int[] nums = { 5 };
        
        solution.ReverseArray(nums);
        
        int[] expected = { 5 };
        CollectionAssert.AreEqual(expected, nums);
    }
}
```

### Problem 2: Contains Duplicate
```csharp
public class Solution {
    // Time: O(n), Space: O(n)
    public bool ContainsDuplicate(int[] nums)
    {
        var seen = new HashSet<int>();
        
        foreach (int num in nums)
        {
            if (seen.Contains(num))
                return true;
            
            seen.Add(num);
        }
        
        return false;
    }
}

[TestClass]
public class ContainsDuplicateTests
{
    [TestMethod]
    [DataRow(new int[] { 1, 2, 3, 1 }, true)]
    [DataRow(new int[] { 1, 2, 3, 4 }, false)]
    [DataRow(new int[] { 99, 99 }, true)]
    [DataRow(new int[] { }, false)]
    public void ContainsDuplicate_WithVariousInputs_ReturnsCorrectResult(
        int[] nums, 
        bool expected)
    {
        var solution = new Solution();
        Assert.AreEqual(expected, solution.ContainsDuplicate(nums));
    }
}
```

### Problem 3: First Unique Character
```csharp
public class Solution {
    // Time: O(n), Space: O(n)
    public int FirstUniqueChar(string s)
    {
        var charCount = new Dictionary<char, int>();
        
        // Count occurrences
        foreach (char c in s)
        {
            if (charCount.ContainsKey(c))
                charCount[c]++;
            else
                charCount[c] = 1;
        }
        
        // Find first char with count 1
        for (int i = 0; i < s.Length; i++)
        {
            if (charCount[s[i]] == 1)
                return i;
        }
        
        return -1;
    }
}

[TestClass]
public class FirstUniqueCharTests
{
    [TestMethod]
    [DataRow("leetcode", 0)]      // 'l' is first unique
    [DataRow("loveleetcode", 2)]  // 'v' is first unique
    [DataRow("aabb", -1)]         // No unique char
    public void FirstUniqueChar_WithVariousInputs_ReturnsCorrectIndex(
        string s, 
        int expected)
    {
        var solution = new Solution();
        Assert.AreEqual(expected, solution.FirstUniqueChar(s));
    }
}
```

### Problem 4: Remove Duplicates from Sorted Array
```csharp
public class Solution {
    // Time: O(n), Space: O(1) in-place
    public int RemoveDuplicates(int[] nums)
    {
        if (nums.Length == 0)
            return 0;
        
        int writeIndex = 0;
        
        for (int readIndex = 1; readIndex < nums.Length; readIndex++)
        {
            if (nums[readIndex] != nums[writeIndex])
            {
                writeIndex++;
                nums[writeIndex] = nums[readIndex];
            }
        }
        
        return writeIndex + 1;  // Return length of array without duplicates
    }
}

[TestClass]
public class RemoveDuplicatesTests
{
    [TestMethod]
    public void RemoveDuplicates_WithDuplicates_ReturnsCount()
    {
        var solution = new Solution();
        int[] nums = { 1, 1, 2 };
        
        int length = solution.RemoveDuplicates(nums);
        
        Assert.AreEqual(2, length);
        // nums should be [1, 2, _] (third element doesn't matter)
        Assert.AreEqual(1, nums[0]);
        Assert.AreEqual(2, nums[1]);
    }
}
```

---

## 8. UNIT TESTING BEST PRACTICES

### ✅ GOOD: Well-Structured Tests
```csharp
public class EmailServiceTests
{
    private readonly EmailService _service;
    private readonly Mock<IEmailRepository> _repositoryMock;
    
    public EmailServiceTests()
    {
        _repositoryMock = new Mock<IEmailRepository>();
        _service = new EmailService(_repositoryMock.Object);
    }
    
    [Fact]
    public async Task ValidateAsync_WithValidEmail_ReturnsSuccess()
    {
        // Arrange
        var email = "user@example.com";
        _repositoryMock
            .Setup(r => r.GetValidatedEmailAsync(It.IsAny<string>()))
            .ReturnsAsync((string)null);
        
        // Act
        var result = await _service.ValidateAsync(email);
        
        // Assert
        Assert.True(result.Success);
        Assert.Equal(email.ToLowerInvariant(), result.Data.NormalizedEmail);
        
        // Verify repository was called
        _repositoryMock.Verify(
            r => r.SaveValidatedEmailAsync(It.IsAny<string>()), 
            Times.Once);
    }
    
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task ValidateAsync_WithEmptyEmail_ReturnsFail(string email)
    {
        // Act
        var result = await _service.ValidateAsync(email);
        
        // Assert
        Assert.False(result.Success);
    }
}
```

---

## 9. COMMON PATTERNS TO KNOW

### Guard Clauses (Cleaner than nested if-else)
```csharp
// ❌ BAD: Nested if-else
public void ProcessEmail(string email)
{
    if (email != null)
    {
        if (!string.IsNullOrWhiteSpace(email))
        {
            if (IsValidEmail(email))
            {
                // Do work
            }
        }
    }
}

// ✅ GOOD: Guard clauses
public void ProcessEmail(string email)
{
    if (email == null)
        throw new ArgumentNullException(nameof(email));
    
    if (string.IsNullOrWhiteSpace(email))
        throw new ArgumentException("Email cannot be empty");
    
    if (!IsValidEmail(email))
        throw new ArgumentException("Invalid email format");
    
    // Do work
}
```

### Try-Parse Pattern
```csharp
// ❌ BAD: Try-catch for expected failures
public int ParseAge(string input)
{
    try
    {
        return int.Parse(input);
    }
    catch (FormatException)
    {
        return -1;
    }
}

// ✅ GOOD: TryParse
public int ParseAge(string input)
{
    if (int.TryParse(input, out int age) && age >= 0)
        return age;
    
    return -1;
}
```

### Null Coalescing
```csharp
// ❌ BAD
string result;
if (email != null)
    result = email;
else
    result = "unknown@example.com";

// ✅ GOOD
string result = email ?? "unknown@example.com";

// ✅ GOOD: Null-coalescing assignment
email ??= "unknown@example.com";
```

---

## 10. PERFORMANCE TIPS

### StringBuilder for String Operations
```csharp
// ❌ BAD: String concatenation in loop (creates new string each time)
string result = "";
foreach (var email in emails)
{
    result += email + ",";  // O(n²) complexity!
}

// ✅ GOOD: StringBuilder
var sb = new StringBuilder();
foreach (var email in emails)
{
    sb.Append(email).Append(",");  // O(n) complexity
}
string result = sb.ToString().TrimEnd(',');
```

### Using Statement for Resources
```csharp
// ✅ GOOD: Automatic disposal
using (var stream = new FileStream("emails.txt", FileMode.Open))
{
    var emails = await ParseEmailsAsync(stream);
}  // stream is disposed here automatically

// ✅ GOOD: C# 8+ syntax
using var stream = new FileStream("emails.txt", FileMode.Open);
var emails = await ParseEmailsAsync(stream);
```

---

## Interview Checklist for Tomorrow

- [ ] Review Result<T> pattern for error handling
- [ ] Understand async/await and when to use it
- [ ] Know dependency injection basics
- [ ] Be comfortable with LINQ (Where, Select, FirstOrDefault, Any)
- [ ] Understand HashSet vs Dictionary vs List trade-offs
- [ ] Know how to write unit tests with Arrange-Act-Assert
- [ ] Be ready to discuss time/space complexity
- [ ] Remember to ask clarifying questions before coding
- [ ] Handle edge cases (null, empty, boundaries)
- [ ] Write clean, readable code with good names

---

Good luck! You've got this! 🚀
