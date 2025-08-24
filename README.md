# Prueba-Tecnica-Jikkosoft

```mermaid
erDiagram
    USERS ||--o{ POSTS : writes
    USERS ||--o{ COMMENTS : writes
    POSTS ||--o{ COMMENTS : has
    POSTS ||--o{ POST_TAGS : tagged_as
    TAGS  ||--o{ POST_TAGS : used_in

    USERS {
      int id PK
      string name
      string email UNIQUE
      string password_hash
      datetime created_at
    }

    POSTS {
      int id PK
      int user_id FK
      string title
      text content
      datetime created_at
      datetime updated_at
      boolean is_published
    }

    COMMENTS {
      int id PK
      int post_id FK
      int user_id FK
      text content
      datetime created_at
      boolean is_deleted
    }

    TAGS {
      int id PK
      string name UNIQUE
      string slug UNIQUE
      datetime created_at
    }

    POST_TAGS {
      int post_id FK
      int tag_id FK
    }
