export async function call(fn: () => Promise<Response>) {
  try {
    const response = await fn();
        

    if (response.statusText === "OK") {
      return {
        success: true,
        data: response.data,
      };
    };

    return {
      success: false,
      error: "Une erreur est survenue",
    };
  } catch (e: any) {  
    return {
      success: false,
      error: e.response.data,
    };
  }
}
