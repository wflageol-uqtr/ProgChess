export async function call(fn: () => Promise<Response>) {
  try {
    const response = await fn();
    const json = await response.json();    

    if (!response.ok) {
      return {
        success: false,
        error: json.message || 'Une erreur est survenue',
      };
    }

    return {
      success: true,
      data: json.data ?? json,
    };
  } catch (e) {
    return {
      success: false,
      error: 'Une erreur est survenue',
    };
  }
}
