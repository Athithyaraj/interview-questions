using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
