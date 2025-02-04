
CREATE TABLE cards (
	id char(15) PRIMARY KEY,
	balance decimal(10,2),
	created_at datetime NOT NULL,
	updated_at datetime NULL,
	deleted_at datetime NULL,
	user_id bigint not null
);

ALTER TABLE cards ADD  DEFAULT (getutcdate()) FOR created_at
GO

CREATE INDEX idx_balance ON cards(balance);

ALTER TABLE cards  WITH CHECK ADD FOREIGN KEY(user_id)
REFERENCES users (id)
GO
