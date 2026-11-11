# Problem Variations & Follow-Up Interview Questions

## 1. EMAIL VALIDATION VARIATIONS

### Variation 1: Batch Email Validation with Caching

**Follow-up Question:** "What if we had 1 million emails to validate? How would you optimize this?"

```csharp
public class CachedEmailValidator
{
    private readonly HashSet<string> _validCache;
    private readonly HashSet<string> _invalidCache;
    
    public CachedEmailValidator()
    {
        _validCache = new HashSet<string>();
        _invalidCache = new HashSet<string>();
    }
    
    public bool IsValidEmail(string email)
    {
        string normalized = email?.Trim().ToLowerInvariant() ?? "";
        
        // Check cache first
        if (_validCache.Contains(normalized))
            return true;
        
        if (_invalidCache.Contains(normalized))
            return false;
        
        // Validate and cache
        bool isValid = ValidateEmail(normalized);
        
        if (isValid)
            _validCache.Add(normalized);
        else
            _invalidCache.Add(normalized);
        
        return isValid;
    }
    
    private bool ValidateEmail(string email)
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

[TestMethod]
public void IsValidEmail_WithCaching_SecondCallIsImmediate()
{
    var validator = new CachedEmailValidator();
    
    // First call - validates
    var result1 = validator.IsValidEmail("test@example.com");
    
    // Second call - uses cache
    var result2 = validator.IsValidEmail("test@example.com");
    
    Assert.IsTrue(result1);
    Assert.IsTrue(result2);
    // Second call should be much faster (no validation)
}
```

**Interview Points:**
- ✓ Shows you think about performance
- ✓ Understands caching benefits
- ✓ Can optimize for scale
- ✓ Uses appropriate data structures (HashSet)

---

### Variation 2: Email Validation with Domain Whitelist

**Follow-up Question:** "What if we only want to accept emails from certain domains?"

```csharp
public class WhitelistEmailValidator
{
    private readonly HashSet<string> _whitelistedDomains;
    
    public WhitelistEmailValidator(params string[] domains)
    {
        _whitelistedDomains = new HashSet<string>(
            domains.Select(d => d.ToLowerInvariant()),
            StringComparer.OrdinalIgnoreCase);
    }
    
    public (bool IsValid, string Error) ValidateEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return (false, "Email is required");
        
        string trimmed = email.Trim();
        
        try
        {
            var mailAddress = new System.Net.Mail.MailAddress(trimmed);
            string domain = mailAddress.Host.ToLowerInvariant();
            
            if (!_whitelistedDomains.Contains(domain))
                return (false, $"Domain '{domain}' is not allowed");
            
            return (true, null);
        }
        catch (Exception ex)
        {
            return (false, $"Invalid email format: {ex.Message}");
        }
    }
}

[TestClass]
public class WhitelistValidatorTests
{
    private WhitelistEmailValidator _validator;
    
    [TestInitialize]
    public void Setup()
    {
        _validator = new WhitelistEmailValidator(
            "example.com", 
            "company.com");
    }
    
    [TestMethod]
    public void ValidateEmail_WithWhitelistedDomain_ReturnsTrue()
    {
        var (isValid, error) = _validator.ValidateEmail("user@example.com");
        Assert.IsTrue(isValid);
        Assert.IsNull(error);
    }
    
    [TestMethod]
    public void ValidateEmail_WithNonWhitelistedDomain_ReturnsFalse()
    {
        var (isValid, error) = _validator.ValidateEmail("user@other.com");
        Assert.IsFalse(isValid);
        Assert.IsNotNull(error);
        Assert.IsTrue(error.Contains("not allowed"));
    }
}
```

**Interview Points:**
- ✓ Can extend existing solutions
- ✓ Handles business requirements
- ✓ Uses tuple for multiple return values
- ✓ Proper error messages

---

## 2. ARRAY PROBLEMS - VARIATIONS

### Variation 1: Two Sum - Find All Unique Pairs

**Question:** "What if we want ALL unique pairs, not just one?"

```csharp
public class TwoSumAllPairs
{
    public List<(int, int)> FindAllPairs(int[] nums, int target)
    {
        var result = new List<(int, int)>();
        var seen = new HashSet<int>();
        
        foreach (int num in nums)
        {
            int complement = target - num;
            
            if (seen.Contains(complement))
            {
                // Add in sorted order to avoid duplicates
                int min = Math.Min(num, complement);
                int max = Math.Max(num, complement);
                result.Add((min, max));
            }
            
            seen.Add(num);
        }
        
        // Remove duplicate pairs
        return result.Distinct().ToList();
    }
}

[TestMethod]
public void FindAllPairs_WithMultiplePairs_ReturnsAll()
{
    var solver = new TwoSumAllPairs();
    int[] nums = { 1, 5, 7, -1, 5 };
    int target = 6;
    
    var result = solver.FindAllPairs(nums, target);
    
    // Should find (1,5) and (-1,7)
    Assert.AreEqual(2, result.Count);
}
```

---

### Variation 2: Two Sum - With Duplicates

**Question:** "What if the array has duplicate numbers?"

```csharp
public class TwoSumWithDuplicates
{
    public int[] FindPair(int[] nums, int target)
    {
        // Use value -> list of indices
        var indexMap = new Dictionary<int, List<int>>();
        
        for (int i = 0; i < nums.Length; i++)
        {
            if (!indexMap.ContainsKey(nums[i]))
                indexMap[nums[i]] = new List<int>();
            
            indexMap[nums[i]].Add(i);
        }
        
        foreach (int num in nums)
        {
            int complement = target - num;
            
            if (indexMap.ContainsKey(complement))
            {
                if (num == complement)
                {
                    // Same value - need two different indices
                    if (indexMap[num].Count >= 2)
                        return new int[] { indexMap[num][0], indexMap[num][1] };
                }
                else
                {
                    // Different values
                    return new int[] { 
                        indexMap[num][0], 
                        indexMap[complement][0] 
                    };
                }
            }
        }
        
        return new int[] { };
    }
}

[TestMethod]
public void FindPair_WithDuplicates_ReturnsValidIndices()
{
    var solver = new TwoSumWithDuplicates();
    int[] nums = { 3, 3, 4 };
    int target = 6;
    
    var result = solver.FindPair(nums, target);
    
    Assert.AreEqual(2, result.Length);
    Assert.AreEqual(0, result[0]);
    Assert.AreEqual(1, result[1]);
}
```

---

### Variation 3: Two Sum - Sorted Array (Two Pointer)

**Question:** "If the array is sorted, can you do better?"

```csharp
public class TwoSumSorted
{
    // For sorted array, we can use two pointers!
    // Time: O(n), Space: O(1)
    public int[] FindPair(int[] nums, int target)
    {
        int left = 0;
        int right = nums.Length - 1;
        
        while (left < right)
        {
            int sum = nums[left] + nums[right];
            
            if (sum == target)
                return new int[] { left, right };
            else if (sum < target)
                left++;  // Need larger sum
            else
                right--;  // Need smaller sum
        }
        
        return new int[] { };
    }
}

[TestMethod]
public void FindPair_SortedArray_UsesTwoPointers()
{
    var solver = new TwoSumSorted();
    int[] nums = { 2, 7, 11, 15 };
    int target = 9;
    
    var result = solver.FindPair(nums, target);
    
    Assert.AreEqual(0, result[0]);  // 2 is at index 0
    Assert.AreEqual(1, result[1]);  // 7 is at index 1
}
```

**Interview Points:**
- ✓ Knows different approaches for different inputs
- ✓ Recognizes sorted array optimization
- ✓ Can use two-pointer technique
- ✓ Space/time tradeoffs

---

## 3. STRING PROBLEMS - VARIATIONS

### Variation 1: Valid Parentheses - Return Invalid Positions

**Question:** "Can you tell me WHERE the validation fails?"

```csharp
public class ValidParenthesesWithPosition
{
    public (bool IsValid, int ErrorPosition, string ErrorMessage) ValidateWithPosition(string s)
    {
        if (s.Length % 2 != 0)
            return (false, -1, "Odd length - cannot be valid");
        
        var stack = new Stack<(char Bracket, int Position)>();
        var pairs = new Dictionary<char, char>
        {
            { ')', '(' },
            { '}', '{' },
            { ']', '[' }
        };
        
        for (int i = 0; i < s.Length; i++)
        {
            char c = s[i];
            
            if (pairs.ContainsKey(c))  // Closing bracket
            {
                if (stack.Count == 0)
                    return (false, i, $"Closing bracket '{c}' at position {i} has no matching opening bracket");
                
                var (openBracket, openPos) = stack.Pop();
                if (openBracket != pairs[c])
                    return (false, i, $"Mismatched brackets: '{openBracket}' at {openPos} doesn't match '{c}' at {i}");
            }
            else
            {
                stack.Push((c, i));  // Opening bracket
            }
        }
        
        if (stack.Count > 0)
        {
            var (bracket, pos) = stack.Peek();
            return (false, pos, $"Unclosed bracket '{bracket}' at position {pos}");
        }
        
        return (true, -1, null);
    }
}

[TestMethod]
public void ValidateWithPosition_InvalidInput_ReturnsErrorPosition()
{
    var validator = new ValidParenthesesWithPosition();
    
    var (isValid, errorPos, errorMsg) = validator.ValidateWithPosition("([)]");
    
    Assert.IsFalse(isValid);
    Assert.AreEqual(2, errorPos);  // Error at position 2
    Assert.IsTrue(errorMsg.Contains("Mismatched"));
}
```

**Interview Points:**
- ✓ Goes beyond basic solution
- ✓ Provides useful debugging info
- ✓ Stores context (position, character)
- ✓ Clear error messages

---

### Variation 2: Reverse String - With Special Characters

**Question:** "What if we want to reverse only letters, keeping special chars in place?"

```csharp
public class ReverseOnlyLetters
{
    public string ReverseLettersOnly(string s)
    {
        char[] arr = s.ToCharArray();
        int left = 0;
        int right = arr.Length - 1;
        
        while (left < right)
        {
            // Skip non-letters from left
            while (left < right && !char.IsLetter(arr[left]))
                left++;
            
            // Skip non-letters from right
            while (left < right && !char.IsLetter(arr[right]))
                right--;
            
            // Swap letters
            if (left < right)
            {
                char temp = arr[left];
                arr[left] = arr[right];
                arr[right] = temp;
                left++;
                right--;
            }
        }
        
        return new string(arr);
    }
}

[TestMethod]
public void ReverseLettersOnly_WithSpecialChars_ReversesOnlyLetters()
{
    var reverser = new ReverseOnlyLetters();
    
    string result = reverser.ReverseLettersOnly("a-bC-dEf-ghIj");
    
    Assert.AreEqual("j-Ih-gfE-dCba", result);
}
```

---

## 4. FOLLOW-UP INTERVIEW QUESTIONS

### Email Validation Follow-Ups

**Q1: "What about internationalized email addresses?"**
```
Answer: In production, RFC 6531 supports non-ASCII characters.
MailAddress class has limitations, so we'd use a more robust solution
or a third-party library for full RFC compliance.
```

**Q2: "Should we validate that the domain actually exists?"**
```csharp
// Option 1: Check DNS records (production use)
public async Task<bool> DomainExistsAsync(string domain)
{
    try
    {
        var records = await Dns.GetHostAddressesAsync(domain);
        return records.Length > 0;
    }
    catch
    {
        return false;
    }
}

// Option 2: Check MX records (more specific)
public async Task<bool> HasMxRecordsAsync(string domain)
{
    // Use DnsClient library or similar
    // Check for MX (Mail eXchange) records
}
```

**Q3: "How would you handle email verification?"**
```csharp
public class EmailVerificationService
{
    public async Task<string> SendVerificationEmailAsync(string email)
    {
        string token = Guid.NewGuid().ToString();
        await _repository.SaveVerificationTokenAsync(email, token);
        
        string verificationUrl = $"https://app.com/verify?token={token}";
        await _emailService.SendAsync(email, verificationUrl);
        
        return token;
    }
    
    public async Task<bool> VerifyEmailAsync(string token)
    {
        var result = await _repository.GetVerificationResultAsync(token);
        return result?.IsVerified ?? false;
    }
}
```

---

### Array Problem Follow-Ups

**Q1: "What if there are multiple solutions in Two Sum?"**
```
Answer: Return all unique pairs, or return the pair with smallest indices,
or return the pair with the closest values - depends on requirements.
```

**Q2: "Can you do Two Sum with O(1) space?"**
```csharp
// For SORTED array only
public int[] TwoSum_O1Space(int[] nums, int target)
{
    int left = 0;
    int right = nums.Length - 1;
    
    while (left < right)
    {
        int sum = nums[left] + nums[right];
        if (sum == target)
            return new int[] { left, right };
        else if (sum < target)
            left++;
        else
            right--;
    }
    return new int[] { };
}
// For unsorted: Not possible - need HashMap or sorting (which costs O(n log n))
```

**Q3: "How would you handle integer overflow in sum?"**
```csharp
public int[] TwoSum_Safe(int[] nums, int target)
{
    for (int i = 0; i < nums.Length; i++)
    {
        // Check for overflow before adding
        if (nums[i] > 0 && target > int.MaxValue - nums[i])
            continue;
        
        int complement = target - nums[i];
        // Rest of logic
    }
    return new int[] { };
}
```

---

### String Problem Follow-Ups

**Q1: "What if we have nested different bracket types?"**
```
Answer: Stack solution already handles this.
Example: "{[()]}" is valid
Example: "{[(])}" is invalid
```

**Q2: "Can you optimize space complexity?"**
```
For basic cases: No, we need Stack to track order.
For simple cases like balanced parens only: 
Could use counter instead of Stack.
```

**Q3: "What about performance with very long strings?"**
```
Stack operations are O(1), loop is O(n).
Overall O(n) time, O(n) space - this is optimal.
Could use streaming if string is too large for memory.
```

---

## 5. UNEXPECTED FOLLOW-UP PATTERNS

### Pattern 1: Interviewer Introduces Bug

**Interviewer:** "I found a bug in your code. Can you spot it?"

```csharp
// Your original code
public int[] TwoSum(int[] nums, int target)
{
    var seen = new Dictionary<int, int>();
    
    for (int i = 0; i < nums.Length; i++)
    {
        int complement = target - nums[i];
        
        if (seen.ContainsKey(complement))
            return new int[] { seen[complement], i };
        
        // BUG HERE: If nums[i] already exists, overwrite it!
        seen[nums[i]] = i;  // Should use: if (!seen.ContainsKey(nums[i]))
    }
    
    return new int[] { };
}

// Test case that breaks it:
[TestMethod]
public void TwoSum_WithDuplicateValues_FindsCorrectPair()
{
    int[] nums = { 3, 3 };
    int target = 6;
    
    var result = TwoSum(nums, target);
    
    // Should return [0, 1]
    // But overwrite bug might cause issues with other inputs
}
```

**How to respond:**
1. "Let me trace through the logic..."
2. "I see - the HashMap overwrites previous indices"
3. "This could cause issues if we have duplicate values"
4. "The fix is to check before adding: `if (!seen.ContainsKey(nums[i]))`"

---

### Pattern 2: Time/Space Trade-off

**Interviewer:** "Can we do this with less space?"

```csharp
// Original: O(n) time, O(n) space
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

// Alternative: O(n²) time, O(1) space
public int[] TwoSum_Brute(int[] nums, int target)
{
    for (int i = 0; i < nums.Length; i++)
    {
        for (int j = i + 1; j < nums.Length; j++)
        {
            if (nums[i] + nums[j] == target)
                return new int[] { i, j };
        }
    }
    return new int[] { };
}

// Alternative: O(n log n) time, O(1) space (with sorting)
public int[] TwoSum_TwoPointer(int[] nums, int target)
{
    // Create array of (value, originalIndex)
    var indexed = nums
        .Select((val, idx) => (val, idx))
        .OrderBy(x => x.val)
        .ToArray();
    
    int left = 0;
    int right = indexed.Length - 1;
    
    while (left < right)
    {
        int sum = indexed[left].val + indexed[right].val;
        if (sum == target)
            return new int[] { indexed[left].idx, indexed[right].idx };
        else if (sum < target)
            left++;
        else
            right--;
    }
    return new int[] { };
}

// Interview answer:
// "HashMap is best for this problem - O(n) time and O(n) space.
//  If space is critical, we could sort (O(n log n)) but that's slower.
//  Brute force is simplest but O(n²) is too slow for production.
//  It depends on the constraint - time or space critical?"
```

---

### Pattern 3: Real-World Scaling

**Interviewer:** "This works for small inputs. What if we had 1 billion emails?"

```csharp
// Original: Single-threaded, all in memory
public List<string> ValidateEmails(List<string> emails)
{
    return emails
        .AsParallel()  // Parallel processing
        .Where(e => !string.IsNullOrWhiteSpace(e))
        .Where(e => IsValidEmail(e.Trim()))
        .ToList();
}

// Better: Batch processing with streaming
public async Task<List<string>> ValidateEmailsBatchAsync(
    IAsyncEnumerable<string> emailStream)
{
    var validEmails = new List<string>();
    const int batchSize = 1000;
    var batch = new List<string>();
    
    await foreach (var email in emailStream)
    {
        batch.Add(email);
        
        if (batch.Count >= batchSize)
        {
            // Process batch
            var valid = batch
                .AsParallel()
                .Where(e => !string.IsNullOrWhiteSpace(e))
                .Where(e => IsValidEmail(e.Trim()))
                .ToList();
            
            validEmails.AddRange(valid);
            batch.Clear();
        }
    }
    
    // Process remaining
    if (batch.Count > 0)
    {
        var valid = batch.Where(e => IsValidEmail(e)).ToList();
        validEmails.AddRange(valid);
    }
    
    return validEmails;
}

// Or distributed: Using cloud queue
public async Task ValidateEmailsDistributedAsync(string queueConnectionString)
{
    var queueClient = new QueueClient(
        new Uri(queueConnectionString),
        new DefaultAzureCredential());
    
    await foreach (QueueMessage message in queueClient.ReceiveMessagesAsync())
    {
        var email = Encoding.UTF8.GetString(message.Body);
        bool isValid = IsValidEmail(email);
        
        // Save to database
        await _db.SaveValidationResultAsync(email, isValid);
        
        await queueClient.DeleteMessageAsync(message.MessageId, message.PopReceipt);
    }
}
```

---

## 6. HOW TO HANDLE COMMON INTERVIEW SCENARIOS

### Scenario 1: You Get Stuck

**What to say:**
```
"Let me think about this for a moment..."
"Can I work through an example first?"
"I'm considering two approaches - let me explain both"
"I'm not immediately sure - could I clarify the constraint?"
"Let me start with a brute force approach, then optimize"
```

**What NOT to say:**
```
"I don't know" (at the start - try first)
"That's not how I learned it" (be adaptable)
"This is unfair" (stay professional)
"I knew this yesterday" (excuses don't help)
```

---

### Scenario 2: You Make a Mistake

**What to do:**
```
1. Stop and acknowledge it
2. Walk through the logic again
3. Test with an example that breaks it
4. Explain the fix
5. Implement the fix
```

**Example:**
```
You: "Wait, I see an issue in my loop condition"
You: "If the array is empty, length is 0, so 0 < 0 is false - it won't enter"
You: "That's actually correct - we'd return the initial result"
You: "Let me verify with a test case..."
```

---

### Scenario 3: Interview Asks "Any Questions?"

**Good questions to ask:**
```
"What does the team use for testing?"
"How large are typical datasets?"
"What's the performance requirement?"
"Do you have specific edge cases to worry about?"
"How is error handling typically done?"
"What logging/monitoring practices do you use?"
"Can I ask about the tech stack?"
```

**Bad questions to ask:**
```
"How much does it pay?" (not in technical interview)
"When do I start?" (premature)
"Do I need to work weekends?" (save for HR)
"Is the code well-tested?" (slightly defensive)
```

---

## Final Checklist

Before saying "I'm done":

- [ ] Code compiles and runs
- [ ] Tested with provided example
- [ ] Tested with at least 2 edge cases
- [ ] Explained time complexity
- [ ] Explained space complexity
- [ ] Code is readable with good naming
- [ ] Added comments for non-obvious parts
- [ ] Discussed potential optimizations
- [ ] Ready for follow-up questions

Good luck! You're prepared for this! 💪🚀
