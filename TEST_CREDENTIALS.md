# Test User Credentials

Berikut adalah user testing yang sudah dibuat untuk aplikasi:

## User Accounts

### 1. Super Admin
- **Email**: testadmin@pertamina.com
- **Password**: password123
- **Role**: SuperAdmin
- **Permissions**: Full access (semua policies)
- **Use Case**: Testing fitur management, user management, full CRUD operations

### 2. Supervisor
- **Email**: supervisor@pertamina.com
- **Password**: password123
- **Role**: Supervisor
- **Permissions**: View dan manage workstreams, milestones
- **Use Case**: Testing approval workflow, milestone monitoring

### 3. Working Level / PIC
- **Email**: pic@pertamina.com
- **Password**: password123
- **Role**: WorkingLevel
- **Permissions**: Edit milestones, view project data
- **Use Case**: Testing data entry, milestone updates

## Project Seed Data

Terdapat sample project di database:
- **Name**: Transformasi BUMN 2026
- **Code**: TRANS-2026
- **Description**: Program transformasi untuk mencapai target 288 BUMN
- **Period**: 1 Januari 2026 - 31 Desember 2026
- **Status**: Active

## Cara Testing

### 1. Login ke Frontend (http://localhost:4200)
1. Buka browser ke http://localhost:4200
2. Masukkan salah satu email dan password di atas
3. Klik "Login"
4. Anda akan diarahkan ke dashboard

### 2. Testing API Langsung
```bash
# Login dan dapatkan token
curl -X POST http://localhost:5056/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"email":"testadmin@pertamina.com","password":"password123"}'

# Gunakan token untuk akses API lainnya
# Copy token dari response login, lalu:
curl -X GET http://localhost:5056/api/projects \
  -H "Authorization: Bearer YOUR_TOKEN_HERE"
```

## Database Info

- **Database**: db_transforma
- **Host**: localhost:5432
- **Username**: postgres
- **Password**: 2MinutesToMidnight#

## Roles Available

1. **SuperAdmin** (ID: 11111111-1111-1111-1111-111111111111)
   - Full system access
   - User management
   - All CRUD operations

2. **Supervisor** (ID: 22222222-2222-2222-2222-222222222222)
   - Milestone approval
   - Workstream management
   - Report generation

3. **WorkingLevel** (ID: 33333333-3333-3333-3333-333333333333)
   - Milestone edit
   - Project view
   - Workstream view

4. **InternalViewer** (ID: 44444444-4444-4444-4444-444444444444)
   - Read-only access for internal Pertamina staff

5. **ExternalViewer** (ID: 55555555-5555-5555-5555-555555555555)
   - Limited read-only access for external stakeholders

## Notes

- Semua user menggunakan password yang sama: `password123`
- Password di-hash menggunakan BCrypt untuk keamanan
- Token JWT expired dalam 480 menit (8 jam)
- Project access perlu diassign manual jika ingin user bisa akses project tertentu
