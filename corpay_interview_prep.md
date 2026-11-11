# Corpay Software Engineer Interview Preparation Guide

## Interview Overview
**Format:** Live coding with pair programming style  
**Tools:** VS Code with screen sharing  
**Duration:** ~60 minutes  
**Topics:** C# coding challenges, email validation, string manipulation, unit tests  

---

## Part 1: Email Validation & String Transformation

### The Problem (Based on HR Mention)
You'll likely need to:
1. Trim input string (remove whitespace)
2. Check and transform the string
3. Validate email format
4. Handle edge cases

### Key Requirements
- Handle leading/trailing whitespace
- Handle empty/null inputs
- Validate email format (basic RFC standard)
- Consider case normalization if needed
- Return meaningful results or error states

### Example Solution with Unit Tests

```csharp
public class EmailValidator
{
    /// <summary>
    /// Validates and transforms an email address
    /// </summary>
    public static (bool IsValid, string NormalizedEmail, string ErrorMessage) ValidateEmail(string input)
    {
        // Handle null or empty
        if (input == null)
            return (false, null, "Input is null");
        
        // Trim whitespace
        string trimmed = input.Trim();
        
        if (string.IsNullOrEmpty(trimmed))
            return (false, null, "Input is empty after trimming");
        
        // Normalize: lowercase for consistency
        string normalized = trimmed.ToLowerInvariant();
        
        // Basic email validation
        if (!IsValidEmailFormat(normalized))
            return (false, null, "Invalid email format");
        
        return (true, normalized, null);
    }
    
    private static bool IsValidEmailFormat(string email)
    {
        // Check basic structure: local@domain.extension
        if (!email.Contains("@"))
            return false;
        
        var parts = email.Split('@');
        
        // Must have exactly one @
        if (parts.Length != 2)
            return false;
        
        string localPart = parts[0];
        string domainPart = parts[1];
        
        // Validate local part (before @)
        if (string.IsNullOrWhiteSpace(localPart) || localPart.Length > 64)
            return false;
        
        // Local part cannot start/end with dot
        if (localPart.StartsWith(".") || localPart.EndsWith("."))
            return false;
        
        // Check for consecutive dots
        if (localPart.Contains(".."))
            return false;
        
        // Validate domain part (after @)
        if (string.IsNullOrWhiteSpace(domainPart) || domainPart.Length > 255)
            return false;
        
        // Domain must have at least one dot
        if (!domainPart.Contains("."))
            return false;
        
        // Domain cannot start/end with dot or hyphen
        if (domainPart.StartsWith(".") || domainPart.EndsWith(".") || 
            domainPart.StartsWith("-") || domainPart.EndsWith("-"))
            return false;
        
        // Check for consecutive dots in domain
        if (domainPart.Contains(".."))
            return false;
        
        // Extension must be at least 2 characters
        string[] domainParts = domainPart.Split('.');
        if (domainParts[domainParts.Length - 1].Length < 2)
            return false;
        
        return true;
    }
}

// Unit Tests (xUnit style)
public class EmailValidatorTests
{
    [Fact]
    public void ValidateEmail_WithValidEmail_ReturnsTrue()
    {
        // Arrange
        string input = "user@example.com";
        
        // Act
        var (isValid, normalized, error) = EmailValidator.ValidateEmail(input);
        
        // Assert
        Assert.True(isValid);
        Assert.Equal("user@example.com", normalized);
        Assert.Null(error);
    }
    
    [Fact]
    public void ValidateEmail_WithWhitespace_TrimmsAndValidates()
    {
        // Arrange
        string input = "  user@example.com  ";
        
        // Act
        var (isValid, normalized, error) = EmailValidator.ValidateEmail(input);
        
        // Assert
        Assert.True(isValid);
        Assert.Equal("user@example.com", normalized);
    }
    
    [Fact]
    public void ValidateEmail_WithUppercase_Normalizes()
    {
        // Arrange
        string input = "USER@EXAMPLE.COM";
        
        // Act
        var (isValid, normalized, error) = EmailValidator.ValidateEmail(input);
        
        // Assert
        Assert.True(isValid);
        Assert.Equal("user@example.com", normalized);
    }
    
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void ValidateEmail_WithNullOrEmpty_ReturnsFalse(string input)
    {
        // Act
        var (isValid, normalized, error) = EmailValidator.ValidateEmail(input);
        
        // Assert
        Assert.False(isValid);
        Assert.NotNull(error);
    }
    
    [Theory]
    [InlineData("notanemail")]
    [InlineData("user@")]
    [InlineData("@example.com")]
    [InlineData("user@@example.com")]
    [InlineData("user@.com")]
    [InlineData("user@domain")]
    [InlineData("user..name@example.com")]
    [InlineData(".user@example.com")]
    [InlineData("user.@example.com")]
    public void ValidateEmail_WithInvalidFormat_ReturnsFalse(string input)
    {
        // Act
        var (isValid, normalized, error) = EmailValidator.ValidateEmail(input);
        
        // Assert
        Assert.False(isValid);
        Assert.NotNull(error);
    }
}
```

---

## Part 2: LeetCode-Style C# Problems

### Problem 1: Two Sum
```csharp
public class Solution {
    public int[] TwoSum(int[] nums, int target) {
        // Use a dictionary to store value -> index mapping
        var seen = new Dictionary<int, int>();
        
        for (int i = 0; i < nums.Length; i++) {
            int complement = target - nums[i];
            
            if (seen.ContainsKey(complement)) {
                return new int[] { seen[complement], i };
            }
            
            // Store the current number and its index
            if (!seen.ContainsKey(nums[i])) {
                seen[nums[i]] = i;
            }
        }
        
        return new int[] { }; // No solution found
    }
}

[TestClass]
public class TwoSumTests {
    [TestMethod]
    public void TwoSum_WithValidInput_ReturnsIndices() {
        var solution = new Solution();
        int[] nums = { 2, 7, 11, 15 };
        int target = 9;
        
        int[] result = solution.TwoSum(nums, target);
        
        Assert.AreEqual(0, result[0]);
        Assert.AreEqual(1, result[1]);
    }
    
    [TestMethod]
    public void TwoSum_WithMultipleSolutions_ReturnsFirst() {
        var solution = new Solution();
        int[] nums = { 3, 2, 4 };
        int target = 6;
        
        int[] result = solution.TwoSum(nums, target);
        
        Assert.AreEqual(1, result[0]);
        Assert.AreEqual(2, result[1]);
    }
}
```

### Problem 2: Valid Parentheses
```csharp
public class Solution {
    public bool IsValid(string s) {
        if (s.Length % 2 != 0) return false; // Odd length can't be valid
        
        var stack = new Stack<char>();
        var pairs = new Dictionary<char, char> {
            { ')', '(' },
            { '}', '{' },
            { ']', '[' }
        };
        
        foreach (char c in s) {
            if (pairs.ContainsKey(c)) {
                // Closing bracket
                if (stack.Count == 0 || stack.Pop() != pairs[c]) {
                    return false;
                }
            } else {
                // Opening bracket
                stack.Push(c);
            }
        }
        
        return stack.Count == 0;
    }
}

[TestClass]
public class ValidParenthesesTests {
    [TestMethod]
    [DataRow("()", true)]
    [DataRow("()[]{}", true)]
    [DataRow("(]", false)]
    [DataRow("([)]", false)]
    [DataRow("{[]}", true)]
    [DataRow("", true)]
    public void IsValid_VariousInputs_ReturnsCorrectResult(string input, bool expected) {
        var solution = new Solution();
        Assert.AreEqual(expected, solution.IsValid(input));
    }
}
```

### Problem 3: Reverse String
```csharp
public class Solution {
    // Do this in-place with O(1) extra space
    public void ReverseString(char[] s) {
        int left = 0;
        int right = s.Length - 1;
        
        while (left < right) {
            // Swap
            char temp = s[left];
            s[left] = s[right];
            s[right] = temp;
            
            left++;
            right--;
        }
    }
}

[TestClass]
public class ReverseStringTests {
    [TestMethod]
    public void ReverseString_WithValidInput_ReversesInPlace() {
        var solution = new Solution();
        char[] input = { 'h', 'e', 'l', 'l', 'o' };
        
        solution.ReverseString(input);
        
        char[] expected = { 'o', 'l', 'l', 'e', 'h' };
        CollectionAssert.AreEqual(expected, input);
    }
}
```

---

## Part 3: String Manipulation Best Practices

### Common String Operations

```csharp
public class StringManipulation {
    // Trim, validate, normalize
    public string ProcessString(string input) {
        if (string.IsNullOrWhiteSpace(input))
            return null;
        
        return input.Trim().ToLowerInvariant();
    }
    
    // Split and process
    public string[] SplitAndTrim(string input, char delimiter) {
        if (string.IsNullOrEmpty(input))
            return new string[] { };
        
        return input.Split(delimiter)
                    .Select(s => s.Trim())
                    .Where(s => !string.IsNullOrEmpty(s))
                    .ToArray();
    }
    
    // Check for special characters
    public bool ContainsSpecialCharacters(string input) {
        return input.Any(c => !char.IsLetterOrDigit(c) && c != '.');
    }
    
    // Pattern matching (email-like)
    public bool MatchesPattern(string input, string pattern) {
        var regex = new System.Text.RegularExpressions.Regex(pattern);
        return regex.IsMatch(input);
    }
}
```

---

## Part 4: Interview Tips & Best Practices

### Before You Start Coding
1. **Clarify requirements** - Ask about edge cases, constraints, expected output format
2. **Think out loud** - Explain your approach before writing code
3. **Discuss trade-offs** - Talk about time/space complexity
4. **Ask about constraints** - Input size, performance requirements

### While Coding
1. **Write readable code** - Use clear variable names
2. **Add comments** - Especially for non-obvious logic
3. **Handle edge cases** - null, empty, negative numbers, special characters
4. **Test as you go** - Mentally walk through your solution
5. **Use meaningful names** - `isValidEmail` not `isValid`

### For Unit Tests
1. **Test happy path** - Basic valid input
2. **Test edge cases** - Empty, null, boundary values
3. **Test error cases** - Invalid inputs
4. **Use descriptive names** - `TestEmailValidation_WithWhitespace_TrimsCorrectly`
5. **Use [Theory] and [InlineData]** - For multiple similar tests

### C# Specific Tips
```csharp
// GOOD: Null checking
if (string.IsNullOrEmpty(input))
    return null;

// GOOD: Using Linq
var results = items.Where(x => x.IsValid)
                    .Select(x => x.Value)
                    .ToList();

// GOOD: Try-catch for parsing
if (!int.TryParse(input, out int result))
    return 0;

// GOOD: Using HashSet for O(1) lookups
var seen = new HashSet<int>();

// GOOD: Using Dictionary for key-value
var mapping = new Dictionary<char, char>();

// GOOD: Tuple for multiple returns
public (bool Success, string Message) ValidateInput(string input) { }

// AVOID: String concatenation in loops
string result = "";
foreach (var item in items) {
    result += item; // BAD - creates new string each time
}

// USE: StringBuilder instead
var sb = new StringBuilder();
foreach (var item in items) {
    sb.Append(item); // GOOD
}
```

---

## Part 5: Mock Interview Scenario

### Sample Problem: Email Domain Extractor

**Problem Statement:**
Write a function that takes a list of email addresses and returns a dictionary where keys are domain names and values are lists of usernames from that domain.

**Requirements:**
- Handle invalid emails
- Extract domain and username correctly
- Case-insensitive domain matching
- Return case-sensitive usernames

**Example:**
```
Input: ["john@example.com", "Jane@EXAMPLE.com", "invalid-email", "bob@test.org"]
Output: {
    "example.com": ["john", "Jane"],
    "test.org": ["bob"]
}
```

```csharp
public class EmailDomainExtractor {
    public Dictionary<string, List<string>> ExtractByDomain(List<string> emails) {
        var result = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);
        
        if (emails == null || emails.Count == 0)
            return result;
        
        foreach (var email in emails) {
            if (string.IsNullOrWhiteSpace(email))
                continue;
            
            var trimmed = email.Trim();
            
            if (!IsValidEmail(trimmed))
                continue;
            
            var parts = trimmed.Split('@');
            string username = parts[0];
            string domain = parts[1].ToLowerInvariant();
            
            if (!result.ContainsKey(domain)) {
                result[domain] = new List<string>();
            }
            
            result[domain].Add(username);
        }
        
        return result;
    }
    
    private bool IsValidEmail(string email) {
        if (!email.Contains("@"))
            return false;
        
        var parts = email.Split('@');
        if (parts.Length != 2)
            return false;
        
        string localPart = parts[0];
        string domainPart = parts[1];
        
        return !string.IsNullOrWhiteSpace(localPart) && 
               !string.IsNullOrWhiteSpace(domainPart) &&
               domainPart.Contains(".");
    }
}

[TestClass]
public class EmailDomainExtractorTests {
    [TestMethod]
    public void ExtractByDomain_WithValidEmails_ReturnsDomainGrouping() {
        var extractor = new EmailDomainExtractor();
        var emails = new List<string> { 
            "john@example.com", 
            "jane@example.com", 
            "bob@test.org" 
        };
        
        var result = extractor.ExtractByDomain(emails);
        
        Assert.AreEqual(2, result.Count);
        Assert.AreEqual(2, result["example.com"].Count);
        Assert.AreEqual(1, result["test.org"].Count);
    }
    
    [TestMethod]
    public void ExtractByDomain_WithMixedCase_NormalizesDomain() {
        var extractor = new EmailDomainExtractor();
        var emails = new List<string> { 
            "john@EXAMPLE.com", 
            "jane@example.COM" 
        };
        
        var result = extractor.ExtractByDomain(emails);
        
        Assert.AreEqual(1, result.Count);
        Assert.AreEqual(2, result["example.com"].Count);
    }
    
    [TestMethod]
    public void ExtractByDomain_WithInvalidEmails_IgnoresThem() {
        var extractor = new EmailDomainExtractor();
        var emails = new List<string> { 
            "invalid-email", 
            "@nodomain.com", 
            "user@", 
            "valid@example.com" 
        };
        
        var result = extractor.ExtractByDomain(emails);
        
        Assert.AreEqual(1, result.Count);
        Assert.IsTrue(result.ContainsKey("example.com"));
    }
}
```

---

## Study Checklist Before Tomorrow

- [ ] Review email validation logic and common regex patterns
- [ ] Practice writing unit tests with [Fact], [Theory], [InlineData]
- [ ] Review LeetCode easy/medium problems (2-sum, valid parentheses, etc.)
- [ ] Practice explaining your approach before coding
- [ ] Test your VS Code setup and screen sharing
- [ ] Review C# Dictionary, HashSet, Stack operations
- [ ] Practice handling null/empty string edge cases
- [ ] Review string manipulation methods (Trim, Split, Contains, etc.)

---

## During the Interview

1. **Listen carefully** to the problem statement
2. **Ask clarifying questions** - Don't assume
3. **Think before coding** - Outline your approach
4. **Explain as you code** - Talk through your logic
5. **Write tests** - Show you understand unit testing
6. **Handle edge cases** - Don't skip error handling
7. **Review your code** - Check for bugs before submitting
8. **Discuss optimizations** - Be ready to improve your solution

Good luck! 🚀
