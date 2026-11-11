# Additional Code Patterns & Changes for Interview

## 1. REFACTORING EXAMPLES - Before & After

### ❌ BAD CODE → ✅ GOOD CODE

#### Example 1: Email Validation Refactoring

**❌ BEFORE (Messy)**
```csharp
public static bool ValidateEmail(string email)
{
    if (email != null && email.Trim() != "")
    {
        string e = email.Trim();
        if (e.Contains("@"))
        {
            string[] parts = e.Split('@');
            if (parts.Length == 2)
            {
                if (parts[0].Length > 0 && parts[1].Length > 0)
                {
                    if (parts[1].Contains("."))
                    {
                        return true;
                    }
                }
            }
        }
    }
    return false;
}
```

**✅ AFTER (Clean)**
```csharp
public static bool ValidateEmail(string email)
{
    // Guard clauses - fail fast
    if (string.IsNullOrWhiteSpace(email))
        return false;
    
    string trimmed = email.Trim();
    
    // Extract parts
    var parts = trimmed.Split('@');
    if (parts.Length != 2)
        return false;
    
    string localPart = parts[0];
    string domainPart = parts[1];
    
    // Validate each part
    return !string.IsNullOrEmpty(localPart) &&
           !string.IsNullOrEmpty(domainPart) &&
           domainPart.Contains(".");
}

// Or even better - use MailAddress
public static bool ValidateEmail(string email)
{
    try
    {
        new System.Net.Mail.MailAddress(email?.Trim() ?? "");
        return true;
    }
    catch
    {
        return false;
    }
}
```

---

#### Example 2: List Processing Refactoring

**❌ BEFORE (Nested loops, inefficient)**
```csharp
public List<string> GetValidEmails(List<string> emails)
{
    List<string> validEmails = new List<string>();
    
    for (int i = 0; i < emails.Count; i++)
    {
        if (emails[i] != null)
        {
            string trimmed = emails[i].Trim();
            
            if (trimmed.Length > 0)
            {
                for (int j = 0; j < trimmed.Length; j++)
                {
                    if (trimmed[j] == '@')
                    {
                        string[] parts = trimmed.Split('@');
                        if (parts.Length == 2 && parts[0].Length > 0 && parts[1].Length > 0)
                        {
                            validEmails.Add(trimmed);
                            break;
                        }
                    }
                }
            }
        }
    }
    
    return validEmails;
}
```

**✅ AFTER (LINQ, clean)**
```csharp
public List<string> GetValidEmails(List<string> emails)
{
    return emails
        .Where(e => !string.IsNullOrWhiteSpace(e))
        .Select(e => e.Trim())
        .Where(e => IsValidEmail(e))
        .ToList();
}

private bool IsValidEmail(string email)
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
```

---

#### Example 3: Dictionary Processing Refactoring

**❌ BEFORE (Verbose)**
```csharp
public Dictionary<string, int> CountEmails(List<string> emails)
{
    Dictionary<string, int> counts = new Dictionary<string, int>();
    
    foreach (string email in emails)
    {
        if (email != null && email.Trim() != "")
        {
            string trimmed = email.Trim().ToLowerInvariant();
            
            if (counts.ContainsKey(trimmed))
            {
                counts[trimmed] = counts[trimmed] + 1;
            }
            else
            {
                counts[trimmed] = 1;
            }
        }
    }
    
    return counts;
}
```

**✅ AFTER (LINQ, concise)**
```csharp
public Dictionary<string, int> CountEmails(List<string> emails)
{
    return emails
        .Where(e => !string.IsNullOrWhiteSpace(e))
        .Select(e => e.Trim().ToLowerInvariant())
        .GroupBy(e => e)
        .ToDictionary(g => g.Key, g => g.Count());
}
```

---

## 2. COMMON MISTAKES & HOW TO FIX THEM

### Mistake 1: Null Reference Exception

**❌ WRONG**
```csharp
public void ProcessEmail(string email)
{
    string trimmed = email.Trim();  // Crashes if email is null!
    // ...
}
```

**✅ CORRECT**
```csharp
public void ProcessEmail(string email)
{
    if (email == null)
        throw new ArgumentNullException(nameof(email));
    
    string trimmed = email.Trim();
    // ...
}

// OR with null-coalescing
public void ProcessEmail(string email)
{
    string trimmed = (email ?? "").Trim();
    // ...
}

// OR with null-conditional
public void ProcessEmail(string email)
{
    string trimmed = email?.Trim() ?? "";
    // ...
}
```

---

### Mistake 2: String Comparison Case Sensitivity

**❌ WRONG**
```csharp
if (domain == "EXAMPLE.COM")  // Fails if domain is "example.com"
{
    // Process
}

// Later in code
dict[domain]  // "example.com" and "EXAMPLE.COM" are different keys!
```

**✅ CORRECT**
```csharp
if (domain.Equals("example.com", StringComparison.OrdinalIgnoreCase))
{
    // Process
}

// Better - normalize upfront
string normalizedDomain = domain.ToLowerInvariant();
if (normalizedDomain == "example.com")
{
    // Process
}

// For Dictionary - use case-insensitive comparer
var dict = new Dictionary<string, List<string>>(
    StringComparer.OrdinalIgnoreCase);
```

---

### Mistake 3: Modifying Collection While Iterating

**❌ WRONG**
```csharp
foreach (var email in emails)
{
    if (!IsValid(email))
        emails.Remove(email);  // Causes skips and crashes!
}
```

**✅ CORRECT**
```csharp
// Option 1: Filter to new list
var validEmails = emails
    .Where(e => IsValid(e))
    .ToList();

// Option 2: Use RemoveAll
emails.RemoveAll(e => !IsValid(e));

// Option 3: Iterate backwards
for (int i = emails.Count - 1; i >= 0; i--)
{
    if (!IsValid(emails[i]))
        emails.RemoveAt(i);
}
```

---

### Mistake 4: Off-By-One Errors

**❌ WRONG**
```csharp
// Skips last element
for (int i = 0; i < array.Length - 1; i++)
{
    Process(array[i]);
}

// Crashes on empty array
int lastElement = array[array.Length];  // IndexOutOfRangeException
```

**✅ CORRECT**
```csharp
// Includes all elements
for (int i = 0; i < array.Length; i++)
{
    Process(array[i]);
}

// Safe access
if (array.Length > 0)
{
    int lastElement = array[array.Length - 1];
}

// Or use LINQ
var lastElement = array.LastOrDefault();
```

---

### Mistake 5: Stack/Queue Operations

**❌ WRONG**
```csharp
var stack = new Stack<char>();
char item = stack.Pop();  // Throws if empty!

if (stack.Count > 0)
{
    char top = stack.Peek();  // Still could be wrong thread
}
```

**✅ CORRECT**
```csharp
var stack = new Stack<char>();

// Always check before popping
if (stack.Count > 0)
{
    char item = stack.Pop();
}

// Or use TryPop (C# 8+)
if (stack.TryPop(out char item))
{
    // Process item
}

// Safe peek
char top = stack.Count > 0 ? stack.Peek() : '\0';
```

---

## 3. PERFORMANCE IMPROVEMENTS

### Example 1: String Concatenation

**❌ SLOW - O(n²) complexity**
```csharp
string result = "";
foreach (var email in emails)
{
    result += email + ",";  // Creates new string each time!
}
```

**✅ FAST - O(n) complexity**
```csharp
var sb = new StringBuilder();
foreach (var email in emails)
{
    sb.Append(email).Append(",");
}
string result = sb.ToString();
```

---

### Example 2: Unnecessary LINQ Chains

**❌ SLOWER**
```csharp
var result = emails
    .Where(e => !string.IsNullOrWhiteSpace(e))
    .Where(e => e.Contains("@"))
    .Select(e => e.Trim())
    .Select(e => e.ToLowerInvariant())
    .Where(e => IsValid(e))
    .ToList();
```

**✅ FASTER**
```csharp
var result = emails
    .Where(e => !string.IsNullOrWhiteSpace(e) && e.Contains("@") && IsValid(e.Trim()))
    .Select(e => e.Trim().ToLowerInvariant())
    .ToList();
```

---

### Example 3: Premature ToList()

**❌ SLOWER - Materializes entire collection**
```csharp
public int CountValid(List<string> emails)
{
    var valid = emails
        .Where(e => IsValid(e))
        .ToList();  // Unnecessary materialization
    
    return valid.Count;
}
```

**✅ FASTER - Uses deferred execution**
```csharp
public int CountValid(List<string> emails)
{
    return emails
        .Count(e => IsValid(e));  // Single pass, returns count directly
}
```

---

## 4. ERROR HANDLING VARIATIONS

### Pattern 1: Try-Catch with Logging

```csharp
public async Task<Result<Email>> ValidateEmailAsync(string email)
{
    try
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email is required");
        
        var trimmed = email.Trim();
        var mailAddress = new System.Net.Mail.MailAddress(trimmed);
        
        _logger.LogInformation("Email validated: {email}", trimmed);
        
        return Result<Email>.Ok(new Email { Address = trimmed });
    }
    catch (ArgumentException ex)
    {
        _logger.LogWarning("Invalid email argument: {message}", ex.Message);
        return Result<Email>.Fail($"Invalid input: {ex.Message}");
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Unexpected error validating email");
        return Result<Email>.Fail("Email validation failed");
    }
}
```

---

### Pattern 2: Multiple Try-Catch Blocks

```csharp
public async Task<Result<Email>> ProcessEmailAsync(string email)
{
    // Validation
    try
    {
        ValidateEmail(email);
    }
    catch (ArgumentException ex)
    {
        return Result<Email>.Fail($"Validation failed: {ex.Message}");
    }
    
    // Processing
    try
    {
        var result = await _service.ProcessAsync(email);
        return Result<Email>.Ok(result);
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Processing failed");
        return Result<Email>.Fail("Processing failed");
    }
}
```

---

### Pattern 3: Try-Finally for Cleanup

```csharp
public void ProcessEmails(string filePath)
{
    StreamReader reader = null;
    try
    {
        reader = new StreamReader(filePath);
        string line;
        while ((line = reader.ReadLine()) != null)
        {
            ProcessEmail(line);
        }
    }
    catch (IOException ex)
    {
        Console.WriteLine($"Error reading file: {ex.Message}");
    }
    finally
    {
        reader?.Dispose();  // Always cleanup
    }
}

// Modern C# 8+ - using statement
public void ProcessEmails(string filePath)
{
    using var reader = new StreamReader(filePath);
    string line;
    while ((line = reader.ReadLine()) != null)
    {
        ProcessEmail(line);
    }
    // reader automatically disposed here
}
```

---

## 5. ADVANCED DATA STRUCTURE PATTERNS

### Pattern 1: Dictionary with Default Values

```csharp
// ❌ Wrong - can throw KeyNotFoundException
int count = userCounts[email];  // Crashes if email not in dict

// ✅ Correct - Option 1: ContainsKey check
if (userCounts.ContainsKey(email))
    count = userCounts[email];

// ✅ Correct - Option 2: TryGetValue
if (userCounts.TryGetValue(email, out int count))
{
    // Use count
}

// ✅ Correct - Option 3: GetValueOrDefault (C# 8+)
int count = userCounts.GetValueOrDefault(email, 0);
```

---

### Pattern 2: Nested Dictionary/List

```csharp
// Domain -> List of emails
Dictionary<string, List<string>> domainEmails = 
    new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);

// Add email
string domain = "example.com";
if (!domainEmails.ContainsKey(domain))
    domainEmails[domain] = new List<string>();

domainEmails[domain].Add("user@example.com");

// Or with helper method
void AddEmailToDomain(string email, string domain)
{
    if (!domainEmails.ContainsKey(domain))
        domainEmails[domain] = new List<string>();
    
    domainEmails[domain].Add(email);
}
```

---

### Pattern 3: Dictionary with Lambda Initialization

```csharp
// ✅ Clean - Initialize with key-value pairs
var phonebook = new Dictionary<string, string>
{
    { "alice", "alice@example.com" },
    { "bob", "bob@example.com" },
    { "charlie", "charlie@example.com" }
};

// ✅ Using LINQ to Dictionary
var emailCounts = emails
    .GroupBy(e => e)
    .ToDictionary(g => g.Key, g => g.Count());

// ✅ Using Dictionary Initializer (C# 6+)
var users = new Dictionary<int, string>
{
    [1] = "Alice",
    [2] = "Bob",
    [3] = "Charlie"
};
```

---

## 6. LEETCODE VARIATIONS

### Two Sum - Variant 1: Find All Pairs

**Problem: Return ALL pairs that sum to target**

```csharp
public List<(int, int)> TwoSumAll(int[] nums, int target)
{
    var seen = new Dictionary<int, int>();
    var result = new HashSet<string>();
    
    for (int i = 0; i < nums.Length; i++)
    {
        int complement = target - nums[i];
        
        if (seen.ContainsKey(complement))
        {
            // Add in sorted order to avoid duplicates like (2,7) and (7,2)
            var pair = seen[complement] < i 
                ? (seen[complement], i) 
                : (i, seen[complement]);
            
            result.Add($"{pair.Item1},{pair.Item2}");
        }
        
        seen[nums[i]] = i;
    }
    
    return result.Select(p => 
    {
        var parts = p.Split(',');
        return (int.Parse(parts[0]), int.Parse(parts[1]));
    }).ToList();
}
```

---

### Valid Parentheses - Variant 1: Minimum Removals

**Problem: What's the minimum number of brackets to remove?**

```csharp
public int MinRemovalForValidParentheses(string s)
{
    int open = 0;
    int remove = 0;
    
    foreach (char c in s)
    {
        if (c == '(')
        {
            open++;
        }
        else if (c == ')')
        {
            if (open > 0)
            {
                open--;
            }
            else
            {
                remove++;  // Extra closing bracket
            }
        }
    }
    
    return remove + open;  // unmatched opening + unmatched closing
}

[TestMethod]
public void MinRemoval_WithMixed_ReturnsCorrectCount()
{
    Assert.AreEqual(1, MinRemovalForValidParentheses("(()"));  // Remove 1 opening
    Assert.AreEqual(1, MinRemovalForValidParentheses("())"));  // Remove 1 closing
    Assert.AreEqual(2, MinRemovalForValidParentheses("(()"));  // etc
}
```

---

### Remove Duplicates - Variant 1: In-Place, Return New Length

```csharp
public int RemoveDuplicates(int[] nums)
{
    if (nums.Length <= 1)
        return nums.Length;
    
    int writeIndex = 0;
    
    for (int i = 1; i < nums.Length; i++)
    {
        if (nums[i] != nums[writeIndex])
        {
            writeIndex++;
            nums[writeIndex] = nums[i];
        }
    }
    
    return writeIndex + 1;
}

[TestMethod]
public void RemoveDuplicates_InPlace_ModifiesArray()
{
    int[] nums = { 0, 0, 1, 1, 1, 2, 2, 3, 3, 4 };
    int length = RemoveDuplicates(nums);
    
    Assert.AreEqual(5, length);
    // nums should be [0, 1, 2, 3, 4, _, _, _, _, _]
    for (int i = 0; i < length; i++)
    {
        Assert.AreEqual(i, nums[i]);
    }
}
```

---

## 7. LAMBDA & FUNCTIONAL PATTERNS

### Pattern 1: Using Func<T, TResult>

```csharp
// Define a validator function
Func<string, bool> isValidEmail = (email) =>
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
};

// Use it
List<string> validEmails = allEmails
    .Where(isValidEmail)
    .ToList();

// Pass it to a method
public List<T> Filter<T>(List<T> items, Func<T, bool> predicate)
{
    return items.Where(predicate).ToList();
}

var result = Filter(emails, isValidEmail);
```

---

### Pattern 2: Action for Side Effects

```csharp
// Define an action
Action<string> logEmail = (email) =>
{
    Console.WriteLine($"Processing: {email}");
};

// Use it
emails.ForEach(logEmail);

// Or inline
emails.ForEach(e => Console.WriteLine($"Email: {e}"));
```

---

## 8. UNIT TEST VARIATIONS

### Variation 1: Testing Exceptions

```csharp
[TestMethod]
[ExpectedException(typeof(ArgumentNullException))]
public void ValidateEmail_WithNull_ThrowsArgumentNullException()
{
    EmailValidator.ValidateEmail(null);
}

// OR with Assert.Throws
[Fact]
public void ValidateEmail_WithNull_ThrowsException()
{
    Assert.Throws<ArgumentNullException>(
        () => EmailValidator.ValidateEmail(null));
}
```

---

### Variation 2: Testing with Setup/Teardown

```csharp
public class EmailServiceTests
{
    private EmailService _service;
    private Mock<IEmailRepository> _repositoryMock;
    
    [SetUp]  // Runs before each test
    public void Setup()
    {
        _repositoryMock = new Mock<IEmailRepository>();
        _service = new EmailService(_repositoryMock.Object);
    }
    
    [TearDown]  // Runs after each test
    public void TearDown()
    {
        _repositoryMock = null;
        _service = null;
    }
    
    [Test]
    public void ValidateEmail_WithValidEmail_CallsRepository()
    {
        var email = "test@example.com";
        
        var result = _service.Validate(email);
        
        _repositoryMock.Verify(
            r => r.Save(It.IsAny<string>()), 
            Times.Once);
    }
}
```

---

### Variation 3: Parameterized Tests with Multiple Data Sources

```csharp
[TestClass]
public class EmailValidatorTests
{
    private static IEnumerable<object[]> ValidEmailData()
    {
        yield return new object[] { "user@example.com", true };
        yield return new object[] { "test@domain.co.uk", true };
        yield return new object[] { "invalid.email", false };
        yield return new object[] { "@nodomain.com", false };
    }
    
    [DynamicData(nameof(ValidEmailData), DynamicDataSourceType.Method)]
    [DataTestMethod]
    public void ValidateEmail_WithVariousInputs_ReturnsExpected(
        string email, 
        bool expected)
    {
        var result = EmailValidator.ValidateEmail(email);
        Assert.AreEqual(expected, result);
    }
}
```

---

## 9. EDGE CASES TO ALWAYS TEST

```csharp
// For email validation
[Theory]
[InlineData(null)]                          // Null
[InlineData("")]                            // Empty
[InlineData("   ")]                         // Whitespace only
[InlineData("user@example.com")]            // Valid
[InlineData("  user@example.com  ")]        // With whitespace
[InlineData("USER@EXAMPLE.COM")]            // Uppercase
[InlineData("user+tag@example.com")]        // With special chars
[InlineData("user@sub.example.com")]        // Subdomain
[InlineData("user@example")]                // No extension
[InlineData("@example.com")]                // No local part
[InlineData("user@")]                       // No domain
[InlineData("user@@example.com")]           // Multiple @
[InlineData("user..name@example.com")]      // Consecutive dots
[InlineData(".user@example.com")]           // Starts with dot
[InlineData("user.@example.com")]           // Ends with dot
public void ValidateEmail_EdgeCases(string email)
{
    var result = EmailValidator.ValidateEmail(email);
    // Assert based on expected validity
}

// For arrays
[Theory]
[InlineData(new int[] { })]                 // Empty array
[InlineData(new int[] { 1 })]               // Single element
[InlineData(new int[] { 1, 2 })]            // Two elements
[InlineData(new int[] { 1, 1, 1, 1 })]      // All duplicates
[InlineData(new int[] { 1, 2, 3, 4, 5 })]   // No duplicates
[InlineData(new int[] { -1, 0, 1 })]        // Negative numbers
public void ArrayOperation_EdgeCases(int[] nums)
{
    var result = Process(nums);
    // Assert
}
```

---

## 10. PRODUCTION-READY PATTERNS

### Pattern 1: Null Object Pattern

```csharp
public class EmailValidationResult
{
    public bool IsValid { get; set; }
    public string NormalizedEmail { get; set; }
    public string ErrorMessage { get; set; }
    
    // Static factory methods (easier than constructors)
    public static EmailValidationResult Valid(string email) =>
        new EmailValidationResult
        {
            IsValid = true,
            NormalizedEmail = email.ToLowerInvariant(),
            ErrorMessage = null
        };
    
    public static EmailValidationResult Invalid(string error) =>
        new EmailValidationResult
        {
            IsValid = false,
            NormalizedEmail = null,
            ErrorMessage = error
        };
}

// Usage
return EmailValidationResult.Valid("user@example.com");
return EmailValidationResult.Invalid("Email format is invalid");
```

---

### Pattern 2: Extension Methods

```csharp
public static class StringExtensions
{
    public static bool IsValidEmail(this string email)
    {
        try
        {
            new System.Net.Mail.MailAddress(email?.Trim() ?? "");
            return true;
        }
        catch
        {
            return false;
        }
    }
    
    public static string NormalizeEmail(this string email) =>
        email?.Trim().ToLowerInvariant() ?? "";
}

// Usage
if (email.IsValidEmail())
{
    string normalized = email.NormalizeEmail();
}
```

---

### Pattern 3: Fluent Interface

```csharp
public class EmailValidator
{
    private readonly string _email;
    
    public EmailValidator(string email)
    {
        _email = email;
    }
    
    public EmailValidator TrimWhitespace()
    {
        // Implementation
        return this;
    }
    
    public EmailValidator NormalizeCasing()
    {
        // Implementation
        return this;
    }
    
    public bool IsValid()
    {
        // Final validation
        return true;
    }
}

// Usage - fluent interface
bool isValid = new EmailValidator(email)
    .TrimWhitespace()
    .NormalizeCasing()
    .IsValid();
```

---

## 11. QUICK COMPARISON - Solutions You Might See

### Two Approaches to Same Problem

```csharp
// Approach 1: Traditional, Explicit
public int[] TwoSum_Traditional(int[] nums, int target)
{
    for (int i = 0; i < nums.Length; i++)
    {
        for (int j = i + 1; j < nums.Length; j++)
        {
            if (nums[i] + nums[j] == target)
            {
                return new int[] { i, j };
            }
        }
    }
    return new int[] { };
}
// Time: O(n²), Space: O(1)

// Approach 2: HashMap, Optimized
public int[] TwoSum_HashMap(int[] nums, int target)
{
    var seen = new Dictionary<int, int>();
    
    for (int i = 0; i < nums.Length; i++)
    {
        int complement = target - nums[i];
        
        if (seen.ContainsKey(complement))
            return new int[] { seen[complement], i };
        
        seen[nums[i]] = i;
    }
    
    return new int[] { };
}
// Time: O(n), Space: O(n)

// Approach 3: LINQ (Less ideal for this, but shows option)
public int[] TwoSum_Linq(int[] nums, int target)
{
    for (int i = 0; i < nums.Length; i++)
    {
        int complement = target - nums[i];
        int index = Array.IndexOf(nums, complement, i + 1);
        
        if (index != -1)
            return new int[] { i, index };
    }
    
    return new int[] { };
}
// Time: O(n²), Space: O(1)
```

**Key Insight:** Always choose Approach 2 (HashMap) - it's O(n) and clean!

---

## Summary: When to Use What

| Problem Type | Data Structure | Why |
|---|---|---|
| Two Sum, Duplicates | HashMap/HashSet | O(1) lookup |
| Matching brackets | Stack | LIFO property |
| Valid sequence | Stack | Verify nesting order |
| Frequency count | Dictionary | Count occurrences |
| Unique items | HashSet | No duplicates |
| Ordered items | List | Index-based access |
| FIFO processing | Queue | First-in-first-out |

---

Good luck with your interview! 🚀
