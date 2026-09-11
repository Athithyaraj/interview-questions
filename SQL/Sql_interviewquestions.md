# SQL Interview Questions and Answers

This guide covers SQL interview questions from beginner to advanced level. The examples use common SQL syntax; some details can vary between SQL Server, PostgreSQL, MySQL, and Oracle.

## SQL Interview Questions

# 1. What is SQL? What are DDL, DML, DCL, TCL, and DQL?

SQL (Structured Query Language) is used to define, query, manipulate, and control data in relational database systems.

- **DDL (Data Definition Language):** Defines database objects. Examples: `CREATE`, `ALTER`, `DROP`, `TRUNCATE`.
- **DML (Data Manipulation Language):** Changes data. Examples: `INSERT`, `UPDATE`, `DELETE`.
- **DCL (Data Control Language):** Controls permissions. Examples: `GRANT`, `REVOKE`.
- **TCL (Transaction Control Language):** Controls transactions. Examples: `COMMIT`, `ROLLBACK`, `SAVEPOINT`.
- **DQL (Data Query Language):** Reads data. The primary command is `SELECT`.

```sql
CREATE TABLE employees (id INT, name VARCHAR(100));
INSERT INTO employees VALUES (1, 'Alice');
SELECT * FROM employees;
```

# 2. What is the difference between `DELETE`, `TRUNCATE`, and `DROP`?

| Command | Purpose | `WHERE` allowed | Object remains? | Typical use |
|---|---|---:|---:|---|
| `DELETE` | Removes selected rows | Yes | Yes | Remove specific records |
| `TRUNCATE` | Removes all rows quickly | No | Yes | Empty a table |
| `DROP` | Removes the table definition and data | No | No | Remove the object |

`DELETE` is a DML operation and is normally logged row by row. `TRUNCATE` is generally faster because it deallocates data pages, but its exact transaction and identity behavior depends on the database engine. `DROP` removes the table itself, including its metadata, indexes, and constraints.

# 3. What is a Primary Key? How is it different from a Unique Key?

A primary key uniquely identifies each row. It cannot contain `NULL`, and a table can have only one primary key, although that key may contain multiple columns.

A unique key also enforces uniqueness, but a table can have multiple unique constraints. Whether a unique constraint permits one or more `NULL` values depends on the database engine.

```sql
CREATE TABLE users (
	user_id INT PRIMARY KEY,
	email VARCHAR(255) UNIQUE
);
```

# 4. What is a Foreign Key?

A foreign key is a column or group of columns that references a primary key or unique key in another table. It enforces referential integrity, preventing child rows from referencing nonexistent parent rows.

```sql
CREATE TABLE orders (
	order_id INT PRIMARY KEY,
	user_id INT,
	FOREIGN KEY (user_id) REFERENCES users(user_id)
);
```

Foreign keys can use actions such as `ON DELETE CASCADE`, `ON DELETE SET NULL`, or `ON DELETE RESTRICT`, depending on the relationship rules.

# 5. What is the difference between `WHERE` and `HAVING`?

`WHERE` filters individual rows before grouping. `HAVING` filters groups after `GROUP BY` and can filter on aggregate results.

```sql
SELECT department_id, COUNT(*) AS employee_count
FROM employees
WHERE status = 'Active'
GROUP BY department_id
HAVING COUNT(*) >= 5;
```

Here, `WHERE` removes inactive employees before counting, while `HAVING` keeps only departments with at least five active employees.

# 6. What is the difference between `GROUP BY` and `ORDER BY`?

`GROUP BY` combines rows with the same values so aggregate functions can be calculated for each group. `ORDER BY` sorts the final result and does not combine rows.

```sql
SELECT department_id, AVG(salary) AS average_salary
FROM employees
GROUP BY department_id
ORDER BY average_salary DESC;
```

# 7. Explain different types of JOINs: `INNER`, `LEFT`, `RIGHT`, `FULL OUTER`, `CROSS`, and `SELF JOIN`.

- **`INNER JOIN`:** Returns only rows that match in both tables.
- **`LEFT JOIN`:** Returns every row from the left table and matching rows from the right table. Missing right-side values are `NULL`.
- **`RIGHT JOIN`:** Returns every row from the right table and matching rows from the left table.
- **`FULL OUTER JOIN`:** Returns all rows from both tables, matching where possible.
- **`CROSS JOIN`:** Returns the Cartesian product, pairing every left row with every right row.
- **`SELF JOIN`:** Joins a table to itself, commonly for employee-manager relationships.

```sql
SELECT e.name, m.name AS manager_name
FROM employees e
LEFT JOIN employees m ON e.manager_id = m.employee_id;
```

Always check join cardinality. An incorrect join condition can multiply rows and produce incorrect totals.

# 8. What is the difference between `UNION` and `UNION ALL`?

Both combine the results of compatible `SELECT` statements. `UNION` removes duplicate rows, which requires extra work. `UNION ALL` keeps duplicates and is usually faster.

```sql
SELECT email FROM customers
UNION ALL
SELECT email FROM suppliers;
```

Both queries must return the same number of columns with compatible data types, and the final `ORDER BY` applies to the combined result.

# 9. What are aggregate functions? Explain `COUNT`, `SUM`, `AVG`, `MIN`, and `MAX`.

Aggregate functions calculate one result from multiple rows:

- `COUNT(*)` counts rows, including rows containing `NULL` values.
- `COUNT(column)` counts non-`NULL` values in that column.
- `SUM(column)` adds numeric values.
- `AVG(column)` returns the average of non-`NULL` numeric values.
- `MIN(column)` and `MAX(column)` return the smallest and largest values.

```sql
SELECT
	COUNT(*) AS employees,
	SUM(salary) AS payroll,
	AVG(salary) AS average_salary,
	MIN(salary) AS minimum_salary,
	MAX(salary) AS maximum_salary
FROM employees;
```

# 10. What is `NULL`? How do `IS NULL` and `COALESCE()` work?

`NULL` means that a value is missing, unknown, or not applicable. It is not equal to zero, an empty string, or another `NULL`. Use `IS NULL` and `IS NOT NULL` to test it because comparisons such as `column = NULL` do not return true.

`COALESCE()` returns the first non-`NULL` expression.

```sql
SELECT name, COALESCE(phone, 'Not provided') AS phone
FROM customers
WHERE phone IS NULL;
```

# 11. What is a subquery? What is a correlated subquery?

A subquery is a query nested inside another query. It can return a scalar value, a list of values, or a derived table.

A correlated subquery references a column from the outer query, so it is logically evaluated for each outer row. It can be expressive but may be slower than an equivalent join or window function.

```sql
SELECT e.name, e.salary
FROM employees e
WHERE e.salary > (
	SELECT AVG(e2.salary)
	FROM employees e2
	WHERE e2.department_id = e.department_id
);
```

# 12. What is the difference between `IN` and `EXISTS`?

`IN` compares a value with a list or subquery result. `EXISTS` checks whether the subquery returns at least one row and can stop searching after finding a match.

```sql
SELECT c.customer_id
FROM customers c
WHERE EXISTS (
	SELECT 1
	FROM orders o
	WHERE o.customer_id = c.customer_id
);
```

`EXISTS` is often a good choice when only existence matters, especially for a correlated relationship. Be careful with `NOT IN` when the subquery can return `NULL`; `NOT EXISTS` avoids that common three-valued-logic trap.

# 13. What are constraints in SQL?

Constraints enforce rules on table data:

- `PRIMARY KEY`: Uniquely identifies rows and disallows `NULL`.
- `FOREIGN KEY`: Enforces relationships between tables.
- `UNIQUE`: Prevents duplicate values.
- `NOT NULL`: Requires a value.
- `CHECK`: Enforces a condition.
- `DEFAULT`: Supplies a value when one is not provided.

```sql
CREATE TABLE products (
	product_id INT PRIMARY KEY,
	price DECIMAL(10, 2) NOT NULL CHECK (price >= 0),
	status VARCHAR(20) DEFAULT 'Active'
);
```

# 14. What is normalization? Explain 1NF, 2NF, and 3NF.

Normalization organizes data to reduce duplication and update anomalies.

- **1NF:** Each column contains atomic values, with no repeating groups.
- **2NF:** The table is in 1NF and every non-key column depends on the whole primary key. This matters especially for composite keys.
- **3NF:** The table is in 2NF and non-key columns do not depend on other non-key columns.

For example, customer details should generally be stored in a customer table rather than repeated on every order. Denormalization can be a deliberate performance choice, but it should be measured and controlled because it increases consistency and update complexity.

# 15. What is an index? What are its advantages and disadvantages?

An index is a data structure that helps the database find rows without scanning the entire table. B-tree indexes are common; other engines also support specialized types such as hash, full-text, or columnstore indexes.

Advantages:

- Faster filtering, joining, sorting, and sometimes grouping.
- Can enforce uniqueness.
- Can support covering queries when required columns are included.

Disadvantages:

- Extra disk space and memory usage.
- Slower `INSERT`, `UPDATE`, and `DELETE` operations because indexes must be maintained.
- Poorly chosen or unused indexes increase maintenance cost.

Index columns used in predicates and joins, inspect execution plans, and avoid indexing every column automatically.

# 16. What is a View? How is it different from a table?

A view is a stored query that behaves like a virtual table. It normally stores the query definition rather than a separate copy of the data.

```sql
CREATE VIEW active_employees AS
SELECT employee_id, name, department_id
FROM employees
WHERE status = 'Active';
```

A table stores data physically. A view can simplify complex queries, provide a stable interface, and restrict exposed columns, but its performance still depends on the underlying query. A materialized or indexed view stores results and must be refreshed or maintained according to the database engine.

# 17. What are Stored Procedures, Functions, and Triggers?

- **Stored procedure:** A named, executable database program that can accept parameters and perform multiple statements. It may return result sets or status values.
- **Function:** Usually returns a value or table and can often be used inside a query. Restrictions vary by database engine.
- **Trigger:** Runs automatically when an event such as `INSERT`, `UPDATE`, or `DELETE` occurs.

Triggers can enforce auditing or derived behavior, but hidden side effects can make debugging and bulk operations difficult. Keep business rules in a clear, testable layer and use database programs where their transaction, security, or data-locality benefits are clear.

# 18. What are transactions? Explain ACID properties.

A transaction is a unit of work that should complete entirely or have no effect.

- **Atomicity:** All operations succeed or all are rolled back.
- **Consistency:** Constraints and database rules remain valid.
- **Isolation:** Concurrent transactions do not expose invalid intermediate states.
- **Durability:** Committed data survives failures according to the database guarantees.

```sql
BEGIN TRANSACTION;
UPDATE accounts SET balance = balance - 100 WHERE account_id = 1;
UPDATE accounts SET balance = balance + 100 WHERE account_id = 2;
COMMIT;
```

Production code should roll back on failure and keep transactions short to reduce locking and contention.

# 19. What are `COMMIT`, `ROLLBACK`, and `SAVEPOINT`?

- `COMMIT` permanently makes the current transaction changes visible according to the database engine.
- `ROLLBACK` undoes uncommitted changes in the transaction.
- `SAVEPOINT` marks a point that allows a partial rollback without undoing the whole transaction.

```sql
BEGIN TRANSACTION;
UPDATE inventory SET quantity = quantity - 1 WHERE product_id = 10;
SAVEPOINT after_inventory;
UPDATE order_items SET quantity = 1 WHERE order_id = 100;
ROLLBACK TO SAVEPOINT after_inventory;
COMMIT;
```

Savepoint syntax differs between database systems.

# 20. What is a CTE (`WITH` clause)? What is a recursive CTE?

A Common Table Expression (CTE) is a named temporary result used by one statement. It improves readability and can simplify multi-step transformations.

```sql
WITH department_totals AS (
	SELECT department_id, SUM(salary) AS total_salary
	FROM employees
	GROUP BY department_id
)
SELECT *
FROM department_totals
WHERE total_salary > 1000000;
```

A recursive CTE references itself and is useful for hierarchies, trees, and sequences. It requires an anchor query and a recursive query, and should include a termination condition.

```sql
WITH RECURSIVE numbers AS (
	SELECT 1 AS number
	UNION ALL
	SELECT number + 1 FROM numbers WHERE number < 5
)
SELECT number FROM numbers;
```

# 21. What are window functions?

Window functions calculate across related rows without collapsing them into one row per group. They use an `OVER` clause and can define ordering and partitions.

```sql
SELECT
	employee_id,
	department_id,
	salary,
	AVG(salary) OVER (PARTITION BY department_id) AS department_average
FROM employees;
```

Common window functions include `ROW_NUMBER`, `RANK`, `DENSE_RANK`, `LAG`, `LEAD`, and running aggregates such as `SUM(...) OVER (...)`.

# 22. Explain `ROW_NUMBER()`, `RANK()`, and `DENSE_RANK()`.

These functions assign positions according to an `ORDER BY` expression:

| Function | Ties receive | Example ranks for salaries 100, 100, 90 |
|---|---|---|
| `ROW_NUMBER()` | Different numbers | 1, 2, 3 |
| `RANK()` | Same rank, gaps after ties | 1, 1, 3 |
| `DENSE_RANK()` | Same rank, no gaps | 1, 1, 2 |

Use `ROW_NUMBER` when exactly one row per position is needed, and `RANK` or `DENSE_RANK` when ties have business meaning.

# 23. What is the difference between `PARTITION BY` and `GROUP BY`?

`GROUP BY` collapses rows into one row per group. `PARTITION BY` divides rows into logical windows while retaining every original row.

```sql
SELECT
	employee_id,
	department_id,
	salary,
	MAX(salary) OVER (PARTITION BY department_id) AS department_max
FROM employees;
```

The query returns every employee and adds the maximum salary for that employee's department.

# 24. What is a composite key?

A composite key uses multiple columns together to uniquely identify a row. It is useful when no single column represents the identity, such as an enrollment identified by both `student_id` and `course_id`.

```sql
CREATE TABLE enrollments (
	student_id INT,
	course_id INT,
	enrolled_on DATE,
	PRIMARY KEY (student_id, course_id)
);
```

Keep composite keys as small and stable as practical because foreign keys and indexes referencing them also contain all key columns.

# 25. What is a candidate key and a super key?

A **super key** is any set of columns that uniquely identifies a row, even if it contains unnecessary columns. A **candidate key** is a minimal super key; removing any column would make it non-unique.

One candidate key is selected as the primary key. Other candidate keys can usually be enforced with unique constraints.

# 26. What is a deadlock?

A deadlock occurs when two or more transactions wait for locks held by one another, so none can continue. The database detects the cycle and normally aborts one transaction as the deadlock victim.

Reduce deadlocks by:

- Accessing tables and rows in a consistent order.
- Keeping transactions short.
- Updating only the required rows.
- Adding appropriate indexes to reduce lock duration.
- Retrying aborted transactions safely when the operation is retryable.

Deadlocks are different from ordinary blocking: blocking may eventually finish, while a deadlock is a cycle that cannot resolve without intervention.

# 27. What are transaction isolation levels?

Isolation levels control how one transaction can observe another transaction's changes. The exact implementation varies by database engine.

- **Read Uncommitted:** May read uncommitted changes, causing dirty reads.
- **Read Committed:** Prevents dirty reads; non-repeatable reads may occur.
- **Repeatable Read:** Protects rows already read; phantom rows may still occur in some engines.
- **Serializable:** Strongest standard isolation; behaves like serial execution but can reduce concurrency.
- **Snapshot or MVCC-based levels:** Read a consistent version of data and can reduce reader-writer blocking, with version-storage costs.

Choose the lowest level that preserves the application's correctness requirements. Higher isolation is not automatically better if it causes unacceptable contention.

# 28. How do you optimize a slow SQL query?

1. Reproduce the problem with realistic data and measure the duration.
2. Inspect the execution plan for scans, expensive sorts, bad estimates, spills, and excessive lookups.
3. Return only needed columns instead of using `SELECT *`.
4. Filter early and make predicates sargable; avoid applying functions to indexed columns when possible.
5. Add or adjust indexes based on the workload, including useful covering columns when appropriate.
6. Check join conditions, data types, cardinality, and statistics.
7. Avoid unnecessary correlated subqueries, repeated work, and accidental Cartesian products.
8. Consider pagination, batching, partitioning, caching, or precomputed data for large workloads.
9. Re-measure after each change and confirm that the improvement holds under concurrency.

Do not add indexes or query hints blindly. The execution plan and measured workload should guide the change.

# 29. What is an execution plan?

An execution plan describes how the database intends to retrieve and process data. It may show scans, seeks, joins, sorts, aggregates, estimated rows, actual rows, memory grants, and operator costs.

An **estimated plan** is generated without executing the query. An **actual plan** includes runtime information and is usually more useful for diagnosing incorrect cardinality estimates, spills, and operators that process far more rows than expected.

Plans are valuable evidence, but displayed cost percentages are relative estimates, not a guarantee that one plan is faster in every environment.

# 30. What is the logical order of SQL query execution?

The conceptual order is generally:

1. `FROM` and `JOIN`
2. `WHERE`
3. `GROUP BY`
4. `HAVING`
5. Window functions and the `SELECT` list
6. `DISTINCT`
7. `ORDER BY`
8. `LIMIT` or `TOP`

This explains why a `SELECT` alias usually cannot be referenced in `WHERE`, while it may be usable in `ORDER BY` depending on the database. The optimizer may physically execute operations in a different order while preserving the query's logical result.

## Most Important SQL Coding Questions

# 1. Find the second-highest salary

The following returns the second distinct salary. If there is no second distinct value, it returns no row.

```sql
SELECT MAX(salary) AS second_highest_salary
FROM employees
WHERE salary < (SELECT MAX(salary) FROM employees);
```

Using `DENSE_RANK` is useful when the full employee rows are required:

```sql
WITH ranked_salaries AS (
	SELECT e.*, DENSE_RANK() OVER (ORDER BY salary DESC) AS salary_rank
	FROM employees e
)
SELECT *
FROM ranked_salaries
WHERE salary_rank = 2;
```

# 2. Find duplicate values

```sql
SELECT email, COUNT(*) AS duplicate_count
FROM employees
GROUP BY email
HAVING COUNT(*) > 1;
```

To return the duplicate rows rather than only the duplicate values:

```sql
WITH duplicate_emails AS (
	SELECT email
	FROM employees
	GROUP BY email
	HAVING COUNT(*) > 1
)
SELECT e.*
FROM employees e
JOIN duplicate_emails d ON d.email = e.email;
```

# 3. Find employees earning more than average salary

```sql
SELECT *
FROM employees
WHERE salary > (SELECT AVG(salary) FROM employees);
```

For the average within each department, correlate the subquery or use a window function:

```sql
SELECT *
FROM (
	SELECT e.*, AVG(salary) OVER (PARTITION BY department_id) AS department_average
	FROM employees e
) ranked
WHERE salary > department_average;
```

# 4. Find highest salary in each department

To return the highest salary value:

```sql
SELECT department_id, MAX(salary) AS highest_salary
FROM employees
GROUP BY department_id;
```

To return every employee tied for the highest salary:

```sql
WITH department_salaries AS (
	SELECT e.*, DENSE_RANK() OVER (
		PARTITION BY department_id
		ORDER BY salary DESC
	) AS salary_rank
	FROM employees e
)
SELECT *
FROM department_salaries
WHERE salary_rank = 1;
```

# 5. Find top 3 salaries in each department

`DENSE_RANK` returns all employees whose salary is within the top three distinct salary values:

```sql
WITH department_salaries AS (
	SELECT e.*, DENSE_RANK() OVER (
		PARTITION BY department_id
		ORDER BY salary DESC
	) AS salary_rank
	FROM employees e
)
SELECT *
FROM department_salaries
WHERE salary_rank <= 3;
```

Use `ROW_NUMBER()` instead when the requirement is exactly three rows per department and a deterministic tie-breaker is available.

# 6. Find the Nth highest salary

Replace `3` with the required value of `N`:

```sql
WITH ranked_salaries AS (
	SELECT e.*, DENSE_RANK() OVER (ORDER BY salary DESC) AS salary_rank
	FROM employees e
)
SELECT *
FROM ranked_salaries
WHERE salary_rank = 3;
```

`DENSE_RANK` treats equal salaries as one rank. If the question means the Nth row rather than the Nth distinct salary, use `ROW_NUMBER()` instead.

## High-Priority Topics for Interviews

Give extra attention to these areas:

- `JOIN`s and join cardinality
- `GROUP BY` and `HAVING`
- Subqueries, `IN`, and `EXISTS`
- CTEs and recursive CTEs
- Window functions and ranking
- Index design and execution plans
- Normalization and data modeling
- Transactions, ACID, isolation, and deadlocks
- Query optimization and performance measurement

For coding rounds, explain your assumptions about duplicates, `NULL` values, ties, missing rows, and database-specific syntax before writing the query.
