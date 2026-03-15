import { notFound } from 'next/navigation';

// catch all routes that start with /docs and render a 404 page for them
export default async function Global404(
    { params }: { params: Promise<{ catchAll?: string[] }> }
) {
  const { catchAll } = await params;
  
  // Define valid routes
  const validRoutes = [
    '/about',
    '/blog',
    '/products'
  ];

  const currentPath = catchAll ? `/${catchAll.join('/')}` : '/';
  const isValid = ['/'].includes(currentPath) || 
                  validRoutes.some(prefix => currentPath.startsWith(prefix));

  if (!isValid) {
    notFound();
  }
  
  return <h1>Valid route: {currentPath}</h1>;
}