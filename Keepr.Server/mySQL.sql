CREATE TABLE IF NOT EXISTS accounts(
  id VARCHAR(255) NOT NULL primary key COMMENT 'primary key',
  createdAt DATETIME DEFAULT CURRENT_TIMESTAMP COMMENT 'Time Created',
  updatedAt DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT 'Last Update',
  name varchar(255) COMMENT 'User Name',
  email varchar(255) COMMENT 'User Email',
  picture varchar(255) COMMENT 'User Picture'
) default charset utf8 COMMENT '';

CREATE TABLE IF NOT EXISTS vaults(
  id INT NOT NULL AUTO_INCREMENT PRIMARY KEY COMMENT 'Primary Key',
  createdAt DATETIME DEFAULT CURRENT_TIMESTAMP COMMENT 'Time Created',
  updatedAt DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT 'Last Update',
  creatorId VARCHAR(255) NOT NULL COMMENT 'FK: User Account',
  name VARCHAR(255) COMMENT 'Vault Name',
  description VARCHAR(255) COMMENT 'Vault Description',
  imgUrl VARCHAR(255) COMMENT 'Vault image',
  isPrivate BOOL COMMENT 'Vault privacy',

  FOREIGN KEY (creatorId) REFERENCES accounts(id) ON DELETE CASCADE 
)default charset utf8 COMMENT '';

CREATE TABLE IF NOT EXISTS keeps(
  id INT NOT NULL AUTO_INCREMENT PRIMARY KEY COMMENT 'Primary Key',
  createdAt DATETIME DEFAULT CURRENT_TIMESTAMP COMMENT 'Time Created',
  updatedAt DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT 'Last Update',
  creatorId VARCHAR(255) NOT NULL COMMENT 'FK: User Account',
  name VARCHAR(255) COMMENT 'Keep Name',
  description VARCHAR(255) COMMENT 'Keep Description',
  img VARCHAR(255) COMMENT 'keep image',
  views INT COMMENT 'Keep Views',
  shares INT COMMENT 'Keep Shares',
  keeps INT COMMENT 'Total keeps',

  FOREIGN KEY (creatorId) REFERENCES accounts(id) ON DELETE CASCADE
)default charset utf8 COMMENT '';

CREATE TABLE IF NOT EXISTS vault_keeps(
  id INT NOT NULL AUTO_INCREMENT PRIMARY KEY COMMENT 'Primary Key',
  createdAt DATETIME DEFAULT CURRENT_TIMESTAMP COMMENT 'Time Created',
  updatedAt DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT 'Last Update',
  creatorId VARCHAR(255) NOT NULL COMMENT 'FK: User Account',
  vaultId INT NOT NULL COMMENT 'FK: Vault Relationship',
  keepId INT NOT NULL COMMENT 'FK: Keep Relationship',

  FOREIGN KEY (creatorId) REFERENCES accounts(id) ON DELETE CASCADE,
  FOREIGN KEY (vaultId) REFERENCES vaults(id) ON DELETE CASCADE,
  FOREIGN KEY (keepId) REFERENCES keeps(id) ON DELETE CASCADE
)default charset utf8 COMMENT '';


 DROP TABLE vault_keeps;
 DROP TABLE vaults;
 DROP TABLE keeps;


