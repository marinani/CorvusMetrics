INSERT INTO users ("Id", "FirstName", "LastName", "Email", "PasswordHash", "Role", "IsActive", "CreatedAtUtc", "UpdatedAtUtc")
VALUES (
  gen_random_uuid(),
  'Admin',
  'Corvus',
  'admin.corvus@corvus.com',
  'jXZbaec2Z0PPMVjzGaf4CTQ9CrkRIPV3htHjjHPAoCpca5KwRrlr+Juz0TY78tpH',
  'Admin',
  true,
  now(),
  null
)
ON CONFLICT ("Email") DO NOTHING;
