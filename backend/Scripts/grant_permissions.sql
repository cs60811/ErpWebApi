-- PostgreSQL 權限設定腳本
-- 請使用有管理員權限的帳號 (如 postgres) 執行此腳本

-- 切換到 LineErpWeb 資料庫後執行以下命令：

-- 方法一：授予 public schema 的權限
GRANT ALL ON SCHEMA public TO "LineErpWeb";
GRANT ALL PRIVILEGES ON ALL TABLES IN SCHEMA public TO "LineErpWeb";
GRANT ALL PRIVILEGES ON ALL SEQUENCES IN SCHEMA public TO "LineErpWeb";

-- 授予未來建立的資料表權限
ALTER DEFAULT PRIVILEGES IN SCHEMA public GRANT ALL ON TABLES TO "LineErpWeb";
ALTER DEFAULT PRIVILEGES IN SCHEMA public GRANT ALL ON SEQUENCES TO "LineErpWeb";

-- 方法二 (可選)：如果上面不夠，可以讓使用者成為資料庫擁有者
-- ALTER DATABASE "LineErpWeb" OWNER TO "LineErpWeb";
