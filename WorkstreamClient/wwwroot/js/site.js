if (await ConfirmDelete()) {
    await DeleteUser(userId);
}
