using System;
using System.Net.Mail;

public class EmailValidator
{
    public static (bool IsValid, string NormalizedEmail, string ErrorMessage) IsEmailValid(string email)
    {
        // Handle null input FIRST
        if (email == null)
            return (false, null, "Email is null");

        // Trim whitespace
        string normalizedEmail = email.Trim();

        // Handle empty after trim
        if (string.IsNullOrEmpty(normalizedEmail))
            return (false, null, "Email is empty");

        try
        {
            // Try to create MailAddress (does validation)
            var mailAddress = new MailAddress(normalizedEmail);
            var host = mailAddress.Host;

            // Additional validation checks
            if (host.Contains(".") && !host.StartsWith(".") && !host.EndsWith("."))
            {
                return (true, normalizedEmail.ToLowerInvariant(), "");
            }
            else
            {
                return (false, normalizedEmail, "Invalid email format");
            }
        }
        catch (Exception ex)
        {
            return (false, normalizedEmail, $"Invalid email: {ex.Message}");
        }
    }
}
public class EmailValidatorTests
{
    [Fact]
    public void ValidEmail_ShouldReturnTrue()
    {
        // Arrange
        var input = "user@example.com";

        // Act
        var result = EmailValidator.Validate(input);

        // Assert
        Assert.True(result.IsValid);
        Assert.Equal("user@example.com", result.Email);
    }

    [Fact]
    public void EmailWithWhitespace_ShouldTrimAndValidate()
    {
        // Arrange
        var input = "  user@example.com  ";

        // Act
        var result = EmailValidator.Validate(input);

        // Assert
        Assert.True(result.IsValid);
        Assert.Equal("user@example.com", result.Email);
    }

    [Fact]
    public void UppercaseEmail_ShouldNormalizeToLowercase()
    {
        // Arrange
        var input = "USER@EXAMPLE.COM";

        // Act
        var result = EmailValidator.Validate(input);

        // Assert
        Assert.True(result.IsValid);
        Assert.Equal("user@example.com", result.Email);
    }

    [Fact]
    public void NullEmail_ShouldReturnFalse()
    {
        // Act
        var result = EmailValidator.Validate(null);

        // Assert
        Assert.False(result.IsValid);
        Assert.Equal("Email is required.", result.Error);
    }

    [Fact]
    public void EmptyEmail_ShouldReturnFalse()
    {
        // Act
        var result = EmailValidator.Validate("");

        // Assert
        Assert.False(result.IsValid);
    }

    [Fact]
    public void WhitespaceOnlyEmail_ShouldReturnFalse()
    {
        // Act
        var result = EmailValidator.Validate("   ");

        // Assert
        Assert.False(result.IsValid);
    }

    [Theory]
    [InlineData("userexample.com")]       // No @
    [InlineData("@example.com")]          // No username
    [InlineData("user@")]                 // No domain
    [InlineData("user@example")]          // No extension
    [InlineData("user@examplecom")]       // No dot in domain
    [InlineData("user@@example.com")]     // Multiple @
    [InlineData("user@.com")]             // Missing domain name
    [InlineData(".user@example.com")]     // Invalid username
    public void InvalidEmails_ShouldReturnFalse(string input)
    {
        // Act
        var result = EmailValidator.Validate(input);

        // Assert
        Assert.False(result.IsValid);
    }
}
public class TwoSumSolution
{
    public int[] TwoSum(int[] nums, int target)
    {
        var completementMap = new Dictionary<int, int>();
        for (int i = 0; i < nums.Length; i++)
        {
            var complement = target - nums[i];
            if (completementMap.ContainsKey(complement))
            {
                return new int[] { completementMap[complement], i };
            }
            if (!completementMap.ContainsKey(nums[i]))
            {
                completementMap[nums[i]] = i;
            }
        }

        throw new int[]{};
    }
}

public class TwoSumSolutionTests
{
    private readonly TwoSumSolution _solution = new();

    [Fact]
    public void TwoSum_ShouldReturnCorrectIndices()
    {
        // Arrange
        int[] nums = { 2, 7, 11, 15 };
        int target = 9;

        // Act
        var result = _solution.TwoSum(nums, target);

        // Assert
        Assert.Equal(new[] { 0, 1 }, result);
    }

    [Fact]
    public void TwoSum_ShouldWorkWithNumbersNotAtBeginning()
    {
        // Arrange
        int[] nums = { 3, 2, 4 };
        int target = 6;

        // Act
        var result = _solution.TwoSum(nums, target);

        // Assert
        Assert.Equal(new[] { 1, 2 }, result);
    }

    [Fact]
    public void TwoSum_ShouldHandleDuplicateNumbers()
    {
        // Arrange
        int[] nums = { 3, 3 };
        int target = 6;

        // Act
        var result = _solution.TwoSum(nums, target);

        // Assert
        Assert.Equal(new[] { 0, 1 }, result);
    }

    [Fact]
    public void TwoSum_ShouldHandleNegativeNumbers()
    {
        // Arrange
        int[] nums = { -3, 4, 3, 90 };
        int target = 0;

        // Act
        var result = _solution.TwoSum(nums, target);

        // Assert
        Assert.Equal(new[] { 0, 2 }, result);
    }

    [Fact]
    public void TwoSum_WhenNoSolution_ShouldReturnEmptyArray()
    {
        // Arrange
        int[] nums = { 1, 2, 3 };
        int target = 10;

        // Act
        var result = _solution.TwoSum(nums, target);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void TwoSum_ShouldHandleNegativeTarget()
    {
        // Arrange
        int[] nums = { -1, -2, -3, -4 };
        int target = -6;

        // Act
        var result = _solution.TwoSum(nums, target);

        // Assert
        Assert.Equal(new[] { 1, 3 }, result);
    }
}

public class ParenthesesValidator
{
public static bool IsValidParentheses(string input)
{
    var stack = new Stack<char>();
    var pairs = new Dictionary<char, char>
    {
        { ')', '(' },
        { '}', '{' },
        { ']', '[' }
    };
    
    // Handle null or empty
    if (string.IsNullOrEmpty(input))
        return true;  // Empty string is valid
    
    // Odd length can't be valid (each opening needs a closing)
    if (input.Length % 2 != 0)
        return false;
    
    foreach (char c in input)
    {
        if (pairs.ContainsKey(c))  // It's a CLOSING bracket
        {
            // Pop from stack and check if it matches
            if (stack.Count == 0 || stack.Pop() != pairs[c])
            {
                return false;  // Either no opening bracket or mismatch
            }
        }
        else  // It's an OPENING bracket
        {
            stack.Push(c);
        }
    }
    
    // At the end, stack should be empty (all matched)
    return stack.Count == 0;
}
}


public class EmailDomainExtractor
{
    /// <summary>
    /// Extracts emails and groups them by domain using MailAddress validation
    /// </summary>
    public Dictionary<string, List<string>> ExtractByDomain(List<string> emails)
    {
        var result = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);
        
        // Handle null or empty input
        if (emails == null || emails.Count == 0)
            return result;
        
        // Loop through each email
        foreach (var email in emails)
        {
            // Skip null or whitespace emails
            if (string.IsNullOrWhiteSpace(email))
                continue;
            
            // Trim whitespace
            var trimmed = email.Trim();
            
            // Validate the email using MailAddress
            if (!IsValidEmail(trimmed))
                continue;  // Skip invalid emails
            
            // Split email into username and domain
            var parts = trimmed.Split('@');
            string username = parts[0];
            string domain = parts[1].ToLowerInvariant();
            
            // If domain doesn't exist in dictionary, create it
            if (!result.ContainsKey(domain))
            {
                result[domain] = new List<string>();
            }
            
            // Add username to the domain's list
            result[domain].Add(username);
        }
        
        return result;
    }
    
    /// <summary>
    /// Validate email using System.Net.Mail.MailAddress
    /// </summary>
    private bool IsValidEmail(string email)
    {
        try
        {
            // MailAddress constructor validates the format
            var mailAddress = new MailAddress(email);
            
            // Additional check: domain must have an extension (dot)
            string host = mailAddress.Host;
            if (!host.Contains("."))
                return false;
            
            return true;
        }
        catch (Exception ex)
        {
            // MailAddress throws FormatException if invalid
            return false;
        }
    }
}

namespace CorpayInterviewPrep
{
    // ============================================
    // EMAIL VALIDATION & STRING TRANSFORMATION
    // ============================================
    
    public class EmailValidator
    {
        public static (bool IsValid, string NormalizedEmail, string ErrorMessage) ValidateEmail(string input)
        {
            // Handle null
            if (input == null)
                return (false, null, "Input is null");
            
            // Trim whitespace
            string trimmed = input.Trim();
            
            if (string.IsNullOrEmpty(trimmed))
                return (false, null, "Input is empty after trimming");
            
            // Normalize
            string normalized = trimmed.ToLowerInvariant();
            
            // Validate format
            if (!IsValidEmailFormat(normalized))
                return (false, null, "Invalid email format");
            
            return (true, normalized, null);
        }
        
        private static bool IsValidEmailFormat(string email)
        {
            if (!email.Contains("@"))
                return false;
            
            var parts = email.Split('@');
            if (parts.Length != 2)
                return false;
            
            string localPart = parts[0];
            string domainPart = parts[1];
            
            // Validate local part
            if (string.IsNullOrWhiteSpace(localPart) || localPart.Length > 64)
                return false;
            
            if (localPart.StartsWith(".") || localPart.EndsWith("."))
                return false;
            
            if (localPart.Contains(".."))
                return false;
            
            // Validate domain part
            if (string.IsNullOrWhiteSpace(domainPart) || domainPart.Length > 255)
                return false;
            
            if (!domainPart.Contains("."))
                return false;
            
            if (domainPart.StartsWith(".") || domainPart.EndsWith("."))
                return false;
            
            if (domainPart.Contains(".."))
                return false;
            
            var domainParts = domainPart.Split('.');
            if (domainParts[domainParts.Length - 1].Length < 2)
                return false;
            
            return true;
        }
    }
    
    // ============================================
    // LEETCODE-STYLE PROBLEMS
    // ============================================
    
    public class ArrayProblems
    {
        // Problem 1: Two Sum
        // Time: O(n), Space: O(n)
        public int[] TwoSum(int[] nums, int target)
        {
            var seen = new Dictionary<int, int>();
            
            for (int i = 0; i < nums.Length; i++)
            {
                int complement = target - nums[i];
                
                if (seen.ContainsKey(complement))
                {
                    return new int[] { seen[complement], i };
                }
                
                if (!seen.ContainsKey(nums[i]))
                {
                    seen[nums[i]] = i;
                }
            }
            
            return new int[] { };
        }
        
        // Problem 2: Majority Element (appears more than n/2 times)
        // Boyer-Moore Voting Algorithm
        // Time: O(n), Space: O(1)
        public int MajorityElement(int[] nums)
        {
            int count = 0;
            int candidate = 0;
            
            foreach (int num in nums)
            {
                if (count == 0)
                {
                    candidate = num;
                }
                
                count += (num == candidate) ? 1 : -1;
            }
            
            return candidate;
        }
        
        // Problem 3: Contains Duplicate
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
    
    public class StringProblems
    {
        // Problem 1: Valid Parentheses
        // Time: O(n), Space: O(n)
        public bool IsValid(string s)
        {
            if (s.Length % 2 != 0)
                return false;
            
            var stack = new Stack<char>();
            var pairs = new Dictionary<char, char>
            {
                { ')', '(' },
                { '}', '{' },
                { ']', '[' }
            };
            
            foreach (char c in s)
            {
                if (pairs.ContainsKey(c))
                {
                    if (stack.Count == 0 || stack.Pop() != pairs[c])
                    {
                        return false;
                    }
                }
                else
                {
                    stack.Push(c);
                }
            }
            
            return stack.Count == 0;
        }
        
        // Problem 2: Reverse String (in-place)
        // Time: O(n), Space: O(1)
        public void ReverseString(char[] s)
        {
            int left = 0;
            int right = s.Length - 1;
            
            while (left < right)
            {
                char temp = s[left];
                s[left] = s[right];
                s[right] = temp;
                
                left++;
                right--;
            }
        }
        
        // Problem 3: First Unique Character in a String
        // Time: O(n), Space: O(n)
        public int FirstUniqChar(string s)
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
            
            // Find first character with count 1
            for (int i = 0; i < s.Length; i++)
            {
                if (charCount[s[i]] == 1)
                    return i;
            }
            
            return -1;
        }
        
        // Problem 4: Palindrome Check
        // Time: O(n), Space: O(1)
        public bool IsPalindrome(string s)
        {
            int left = 0;
            int right = s.Length - 1;
            
            while (left < right)
            {
                // Skip non-alphanumeric characters
                while (left < right && !char.IsLetterOrDigit(s[left]))
                    left++;
                
                while (left < right && !char.IsLetterOrDigit(s[right]))
                    right--;
                
                // Compare characters (case-insensitive)
                if (char.ToLowerInvariant(s[left]) != char.ToLowerInvariant(s[right]))
                    return false;
                
                left++;
                right--;
            }
            
            return true;
        }
    }
    
    // ============================================
    // ADVANCED PROBLEM: Email Domain Extractor
    // ============================================
    
    public class EmailDomainExtractor
    {
        public Dictionary<string, List<string>> ExtractByDomain(List<string> emails)
        {
            var result = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);
            
            if (emails == null || emails.Count == 0)
                return result;
            
            foreach (var email in emails)
            {
                if (string.IsNullOrWhiteSpace(email))
                    continue;
                
                var trimmed = email.Trim();
                
                if (!IsValidEmail(trimmed))
                    continue;
                
                var parts = trimmed.Split('@');
                string username = parts[0];
                string domain = parts[1].ToLowerInvariant();
                
                if (!result.ContainsKey(domain))
                {
                    result[domain] = new List<string>();
                }
                
                result[domain].Add(username);
            }
            
            return result;
        }
        
        private bool IsValidEmail(string email)
        {
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
    
    // ============================================
    // TEST RUNNER
    // ============================================
    
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("═══════════════════════════════════════════════════════");
            Console.WriteLine("CORPAY INTERVIEW PREPARATION - PRACTICE EXAMPLES");
            Console.WriteLine("═══════════════════════════════════════════════════════\n");
            
            // Test Email Validation
            Console.WriteLine("1. EMAIL VALIDATION TESTS");
            Console.WriteLine("─────────────────────────────────────────────────────\n");
            TestEmailValidation();
            
            // Test Array Problems
            Console.WriteLine("\n2. ARRAY PROBLEMS");
            Console.WriteLine("─────────────────────────────────────────────────────\n");
            TestArrayProblems();
            
            // Test String Problems
            Console.WriteLine("\n3. STRING PROBLEMS");
            Console.WriteLine("─────────────────────────────────────────────────────\n");
            TestStringProblems();
            
            // Test Advanced Problem
            Console.WriteLine("\n4. ADVANCED PROBLEM: Email Domain Extractor");
            Console.WriteLine("─────────────────────────────────────────────────────\n");
            TestEmailDomainExtractor();
            
            Console.WriteLine("\n═══════════════════════════════════════════════════════");
            Console.WriteLine("All tests completed! Good luck with your interview! 🚀");
            Console.WriteLine("═══════════════════════════════════════════════════════");
        }
        
        private static void TestEmailValidation()
        {
            var testCases = new[]
            {
                ("user@example.com", true),
                ("  user@example.com  ", true),
                ("USER@EXAMPLE.COM", true),
                (null, false),
                ("", false),
                ("   ", false),
                ("notanemail", false),
                ("user@", false),
                ("@example.com", false),
                ("user@@example.com", false),
                ("user@domain", false),
                ("user..name@example.com", false),
            };
            
            foreach (var (input, expectedValid) in testCases)
            {
                var (isValid, normalized, error) = EmailValidator.ValidateEmail(input);
                
                string displayInput = input == null ? "null" : $"\"{input}\"";
                string status = isValid == expectedValid ? "✓ PASS" : "✗ FAIL";
                
                Console.WriteLine($"{status} | Input: {displayInput}");
                if (isValid)
                    Console.WriteLine($"       Normalized: {normalized}");
                else
                    Console.WriteLine($"       Error: {error}");
                Console.WriteLine();
            }
        }
        
        private static void TestArrayProblems()
        {
            var arrayProblems = new ArrayProblems();
            
            // Two Sum
            Console.WriteLine("Two Sum Problem:");
            int[] nums = { 2, 7, 11, 15 };
            int target = 9;
            var result = arrayProblems.TwoSum(nums, target);
            Console.WriteLine($"Input: [{string.Join(", ", nums)}], Target: {target}");
            Console.WriteLine($"Output: [{string.Join(", ", result)}]");
            Console.WriteLine($"✓ Explanation: nums[{result[0]}] + nums[{result[1]}] = {nums[result[0]]} + {nums[result[1]]} = {target}\n");
            
            // Contains Duplicate
            Console.WriteLine("Contains Duplicate Problem:");
            int[] numsWithDup = { 1, 2, 3, 1 };
            bool hasDuplicate = arrayProblems.ContainsDuplicate(numsWithDup);
            Console.WriteLine($"Input: [{string.Join(", ", numsWithDup)}]");
            Console.WriteLine($"Output: {hasDuplicate}");
            Console.WriteLine($"✓ Explanation: The value 1 appears twice\n");
            
            // Majority Element
            Console.WriteLine("Majority Element Problem:");
            int[] numsMajority = { 3, 2, 3 };
            int majority = arrayProblems.MajorityElement(numsMajority);
            Console.WriteLine($"Input: [{string.Join(", ", numsMajority)}]");
            Console.WriteLine($"Output: {majority}");
            Console.WriteLine($"✓ Explanation: 3 appears {(numsMajority.Count(x => x == 3))} times (more than {numsMajority.Length}/2 = {Math.Ceiling(numsMajority.Length / 2.0)})\n");
        }
        
        private static void TestStringProblems()
        {
            var stringProblems = new StringProblems();
            
            // Valid Parentheses
            Console.WriteLine("Valid Parentheses Problem:");
            var parenTestCases = new[] { ("()", true), ("()[]{}", true), ("(]", false), ("([)]", false) };
            foreach (var (input, expected) in parenTestCases)
            {
                bool result = stringProblems.IsValid(input);
                string status = result == expected ? "✓" : "✗";
                Console.WriteLine($"{status} Input: \"{input}\" => {result}");
            }
            Console.WriteLine();
            
            // Reverse String
            Console.WriteLine("Reverse String Problem:");
            char[] s = { 'h', 'e', 'l', 'l', 'o' };
            Console.WriteLine($"Input: [{string.Join(", ", s)}]");
            stringProblems.ReverseString(s);
            Console.WriteLine($"Output: [{string.Join(", ", s)}]\n");
            
            // First Unique Character
            Console.WriteLine("First Unique Character Problem:");
            string str = "leetcode";
            int uniqueIndex = stringProblems.FirstUniqChar(str);
            Console.WriteLine($"Input: \"{str}\"");
            Console.WriteLine($"Output: {uniqueIndex}");
            Console.WriteLine($"✓ Explanation: The character 'l' at index {uniqueIndex} appears only once\n");
            
            // Palindrome
            Console.WriteLine("Palindrome Check Problem:");
            var palindromeTests = new[] { ("A man, a plan, a canal: Panama", true), ("0P", false) };
            foreach (var (input, expected) in palindromeTests)
            {
                bool result = stringProblems.IsPalindrome(input);
                string status = result == expected ? "✓" : "✗";
                Console.WriteLine($"{status} Input: \"{input}\" => {result}\n");
            }
        }
        
        private static void TestEmailDomainExtractor()
        {
            var extractor = new EmailDomainExtractor();
            
            var emails = new List<string>
            {
                "john@example.com",
                "Jane@EXAMPLE.com",
                "invalid-email",
                "bob@test.org",
                "@nodomain.com",
                "user@domain"
            };
            
            Console.WriteLine("Input emails:");
            foreach (var email in emails)
                Console.WriteLine($"  - {email}");
            
            var result = extractor.ExtractByDomain(emails);
            
            Console.WriteLine("\nOutput (grouped by domain):");
            foreach (var kvp in result)
            {
                Console.WriteLine($"  {kvp.Key}:");
                foreach (var user in kvp.Value)
                {
                    Console.WriteLine($"    - {user}");
                }
            }
            
            Console.WriteLine("\nTest Results:");
            Console.WriteLine($"✓ Valid emails extracted: {result.Sum(x => x.Value.Count)}");
            Console.WriteLine($"✓ Unique domains: {result.Count}");
            Console.WriteLine($"✓ Invalid emails ignored: {emails.Count - result.Sum(x => x.Value.Count)}");
        }
    }
}